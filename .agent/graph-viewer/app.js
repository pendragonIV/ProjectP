const container2D = document.getElementById('mynetwork');
const container3D = document.getElementById('3d-graph');
const nodeInfo = document.getElementById('node-info');

let network2D = null;
let graph3D = null;
let nodesDataset = null;
let edgesDataset = null;

let rawNodes = [];
let rawEdges = [];
let processedNodes = [];
let processedEdges = [];

let currentMode = '2d-force';
let currentRole = 'developer'; // developer | architect
let currentColorMode = 'heatmap'; // feature | heatmap

let tourActive = false;
let tourTimeouts = [];

const groupColors = {
    'core': '#3498db',     // Xanh dương
    'game': '#2ecc71',     // Xanh lá
    'ui': '#e74c3c',       // Đỏ
    'bootstrap': '#9b59b6',// Tím
    'default': '#95a5a6'   // Xám
};

function getGroupColor(group) {
    if (!group) return groupColors['default'];
    const g = group.toLowerCase();
    if (groupColors[g]) return groupColors[g];
    // Sinh màu cố định từ chuỗi
    let hash = 0;
    for (let i = 0; i < group.length; i++) hash = group.charCodeAt(i) + ((hash << 5) - hash);
    const c = (hash & 0x00FFFFFF).toString(16).toUpperCase();
    return '#' + '00000'.substring(0, 6 - c.length) + c;
}

// Base Options cho 2D
const getOptions2D = (isHierarchical = false) => ({
    nodes: {
        shape: 'box',
        margin: 10,
        font: { color: '#ffffff', face: 'Segoe UI' },
        borderWidth: 2,
        shadow: true
    },
    edges: {
        width: 2,
        arrows: 'to',
        font: { color: '#a0a0a0', size: 12, align: 'middle' },
        color: { color: '#666666', highlight: '#4facf7' },
        smooth: { type: 'cubicBezier' }
    },
    layout: {
        hierarchical: isHierarchical ? {
            enabled: true,
            direction: 'UD',
            sortMethod: 'directed',
            nodeSpacing: 150,
            levelSeparation: 150
        } : { enabled: false }
    },
    physics: {
        enabled: !isHierarchical,
        forceAtlas2Based: { gravitationalConstant: -50, centralGravity: 0.01, springLength: 100, springConstant: 0.08 },
        solver: 'forceAtlas2Based',
        stabilization: { iterations: 150 }
    }
});

async function loadGraph() {
    try {
        const data = window.CONTEXT_DATA;
        if (!data) throw new Error("window.CONTEXT_DATA is undefined");

        rawNodes = data.nodes;
        rawEdges = data.edges;

        applyRoleAndProcessData();
    } catch (error) {
        console.error("Lỗi:", error);
        nodeInfo.innerHTML = `<span style="color:#f44336">Lỗi tải dữ liệu.</span>`;
    }
}

function applyRoleAndProcessData() {
    if (currentRole === 'architect') {
        // Gom cụm theo group
        const groupMap = {};
        rawNodes.forEach(n => {
            const g = n.group || 'default';
            if (!groupMap[g]) groupMap[g] = { id: g, label: g.toUpperCase(), group: g, isGroupNode: true, childCount: 0 };
            groupMap[g].childCount++;
        });

        processedNodes = Object.values(groupMap).map(n => ({ 
            ...n, 
            label: `${n.label}\n(${n.childCount} classes)`, 
            hiddenOpacity: 0.2,
            color: getGroupColor(n.group)
        }));
        
        const groupEdges = {};
        rawEdges.forEach(e => {
            const fromNode = rawNodes.find(n => n.id === e.from);
            const toNode = rawNodes.find(n => n.id === e.to);
            if (fromNode && toNode) {
                const gFrom = fromNode.group || 'default';
                const gTo = toNode.group || 'default';
                if (gFrom !== gTo) {
                    const edgeKey = `${gFrom}->${gTo}`;
                    groupEdges[edgeKey] = { from: gFrom, to: gTo };
                }
            }
        });
        processedEdges = Object.values(groupEdges);
    } else {
        // Developer mode
        processedNodes = rawNodes.map(n => ({ 
            ...n, 
            hiddenOpacity: 0.2,
            color: getGroupColor(n.group)
        }));
        processedEdges = rawEdges;
    }

    nodesDataset = new vis.DataSet(processedNodes);
    edgesDataset = new vis.DataSet(processedEdges);

    if (currentMode === '3d-force') {
        init3DGraph();
    } else {
        init2DGraph(currentMode === '2d-tree');
    }
    applyColorMode(); // Đảm bảo đồng bộ màu sau khi khởi tạo
}

function init2DGraph(isHierarchical) {
    if (network2D) network2D.destroy();
    network2D = new vis.Network(container2D, { nodes: nodesDataset, edges: edgesDataset }, getOptions2D(isHierarchical));

    network2D.on("click", function (params) {
        if (params.nodes.length > 0) {
            focusOnNode2D(params.nodes[0]);
        } else {
            resetFocus();
            nodeInfo.innerHTML = "Click on a node to see its details.";
        }
    });

    // Ép Redraw sau khi DOM đã sẵn sàng để tránh lỗi bị cắt ngang
    setTimeout(() => { 
        if(network2D) {
            const container = document.getElementById('graph-container');
            network2D.setSize(container.clientWidth + "px", container.clientHeight + "px");
            network2D.fit(); 
        }
    }, 100);
}

function init3DGraph() {
    const container = document.getElementById('graph-container');
    const w = container.clientWidth;
    const h = container.clientHeight;

    const gData = {
        nodes: JSON.parse(JSON.stringify(processedNodes)), 
        links: processedEdges.map(e => ({ source: e.from, target: e.to }))
    };

    if (graph3D) {
        graph3D.width(w).height(h).graphData(gData);
        return;
    }

    graph3D = ForceGraph3D()(container3D)
        .width(w)
        .height(h)
        .graphData(gData)
        .nodeId('id')
        .nodeLabel('label')
        .linkDirectionalArrowLength(3.5)
        .linkDirectionalArrowRelPos(1)
        .onNodeClick(node => {
            stopTour();
            flyTo3DNode(node);
            displayNodeInfo(node);
        })
        .onBackgroundClick(() => {
            resetFocus();
            nodeInfo.innerHTML = "Click on a node to see its details.";
        });

    if (currentColorMode === 'heatmap') {
        graph3D.nodeVal(node => Math.max((node.complexity || 5) / 2, 2));
    } else {
        graph3D.nodeVal(5);
    }
    
    graph3D.nodeColor(node => {
        if (currentColorMode === 'heatmap') return getHeatmapColor(node.complexity);
        return node.color; // Sử dụng màu Group cố định đã tính
    });

    // Cấu hình Camera Control giống Unity 3D
    const controls = graph3D.controls();
    controls.mouseButtons = {
        LEFT: 0,   // Chuột trái: Orbit (xoay quanh tâm)
        MIDDLE: 2, // Chuột giữa: Pan (dịch chuyển ngang dọc)
        RIGHT: 0   // Chuột phải: Sẽ bị hack thành Look Around (xem mousedown listener)
    };
}

function flyTo3DNode(node) {
    if (!graph3D) return;
    const distance = 100;
    const distRatio = 1 + distance/Math.hypot(node.x, node.y, node.z);
    graph3D.cameraPosition(
        { x: node.x * distRatio, y: node.y * distRatio, z: node.z * distRatio }, 
        node, 2000
    );
}

function switchViewMode(mode) {
    stopTour();
    currentMode = mode;
    if (mode === '3d-force') {
        container2D.style.display = 'none';
        container3D.style.display = 'block';
        if (!graph3D) {
            init3DGraph();
        } else {
            const container = document.getElementById('graph-container');
            graph3D.width(container.clientWidth).height(container.clientHeight);
        }
    } else {
        container3D.style.display = 'none';
        container2D.style.display = 'block';
        init2DGraph(mode === '2d-tree');
    }
}

// ----------------- Logic Focus & Impact -----------------
function focusOnNode2D(nodeId) {
    const connectedNodes = network2D.getConnectedNodes(nodeId);
    connectedNodes.push(nodeId);

    const updateArray = [];
    nodesDataset.forEach(node => {
        updateArray.push({ 
            id: node.id, 
            opacity: connectedNodes.includes(node.id) ? 1.0 : node.hiddenOpacity, 
            color: currentColorMode === 'heatmap' ? { background: getHeatmapColor(node.complexity) } : undefined 
        });
    });
    nodesDataset.update(updateArray);

    const nodeData = nodesDataset.get(nodeId);
    displayNodeInfo(nodeData);
}

function resetFocus() {
    const updateArray = [];
    nodesDataset.forEach(node => { 
        // Trong 2D, nếu là heatmap, phải set lại màu background và border để ghi đè chuỗi màu của group
        let nodeColorObj = undefined;
        if (currentColorMode === 'heatmap') {
            const hc = getHeatmapColor(node.complexity);
            nodeColorObj = { background: hc, border: hc };
        } else {
            // Nếu là feature, trả về màu mặc định của group
            nodeColorObj = { background: node.color, border: node.color };
        }

        updateArray.push({ 
            id: node.id, 
            opacity: 1.0, 
            color: nodeColorObj 
        }); 
    });
    nodesDataset.update(updateArray);
    
    if (currentMode === '3d-force' && graph3D) {
        graph3D.nodeColor(node => {
            if (currentColorMode === 'heatmap') return getHeatmapColor(node.complexity);
            return node.color;
        });
    }
}

window.focusOnGroup = function(groupName) {
    if(currentMode !== '3d-force') {
        const updateArray = [];
        nodesDataset.forEach(node => {
            updateArray.push({ id: node.id, opacity: (node.group === groupName) ? 1.0 : 0.1, color: undefined });
        });
        nodesDataset.update(updateArray);
    }
};

window.showImpact = function(nodeId) {
    // Thuật toán tìm tất cả các node bị ảnh hưởng (các node gọi đến nodeId này)
    // Edge: from -> to nghĩa là `from` dùng `to`. Vậy nếu sửa `to`, `from` bị ảnh hưởng.
    const impacted = new Set();
    const queue = [nodeId];
    
    while(queue.length > 0) {
        const current = queue.shift();
        processedEdges.forEach(e => {
            if (e.to === current && !impacted.has(e.from)) {
                impacted.add(e.from);
                queue.push(e.from);
            }
        });
    }
    
    impacted.add(nodeId);

    if (currentMode === '3d-force') {
        graph3D.nodeColor(node => {
            if (node.id === nodeId) return '#e74c3c'; // Root cause (Red)
            if (impacted.has(node.id)) return '#e67e22'; // Affected (Orange)
            return 'rgba(100,100,100,0.1)'; // Others dim
        });
    } else {
        const updateArray = [];
        nodesDataset.forEach(node => {
            if (node.id === nodeId) {
                updateArray.push({ id: node.id, opacity: 1.0, color: { background: '#e74c3c', border: '#c0392b' } });
            } else if (impacted.has(node.id)) {
                updateArray.push({ id: node.id, opacity: 1.0, color: { background: '#e67e22', border: '#d35400' } });
            } else {
                updateArray.push({ id: node.id, opacity: 0.1, color: undefined });
            }
        });
        nodesDataset.update(updateArray);
    }
};

// ----------------- Sidebar -----------------
function displayNodeInfo(node) {
    let html = `<div><span class="prop-label">ID:</span> ${node.id}</div>
                <div style="margin-top:10px;"><span class="prop-label">${node.isGroupNode ? 'Feature Module' : 'Group'}:</span> ${node.group || 'default'}</div>`;

    if (node.complexity) {
        html += `<div style="margin-top:10px;"><span class="prop-label">Complexity Score:</span> <strong style="color:${getHeatmapColor(node.complexity)}">${node.complexity}</strong></div>`;
    }

    if (node.title) html += `<div style="margin-top:10px;"><span class="prop-label">Description:</span></div>
                             <div style="background-color:#1e1e1e; padding:10px; margin-top:5px; border-radius:4px; border:1px solid #444;">${node.title.replace(/\n/g, '<br>').replace(/\\n/g, '<br>')}</div>`;

    if (node.fields && node.fields.length > 0) html += `<div style="margin-top:10px;"><span class="prop-label">Fields:</span></div><div class="code-list">${node.fields.join('\n')}</div>`;
    if (node.methods && node.methods.length > 0) html += `<div style="margin-top:10px;"><span class="prop-label">Methods:</span></div><div class="code-list">${node.methods.join('\n')}</div>`;

    if (node.filePath) {
        const vscodeUrl = `vscode://file/${node.filePath.replace(/\\/g, '/')}`;
        html += `<a href="${vscodeUrl}" class="ide-btn">Open in VSCode</a>`;
    }

    if (node.group && node.group !== 'default' && currentMode !== '3d-force' && !node.isGroupNode) {
        html += `<button class="ide-btn" style="background-color:#27ae60; margin-top:10px;" onclick="focusOnGroup('${node.group}')">Highlight Feature: ${node.group}</button>`;
    }
    
    html += `<button class="ide-btn" style="background-color:#c0392b; margin-top:10px;" onclick="showImpact('${node.id}')">🔥 Show Downstream Impact</button>`;

    nodeInfo.innerHTML = html;
}

// ----------------- Guided Tour 3D -----------------
function playTour() {
    stopTour(); // Clear previous
    if (currentMode !== '3d-force') {
        document.getElementById('viewModeSelect').value = '3d-force';
        switchViewMode('3d-force');
    }

    tourActive = true;
    const tourNodes = graph3D.graphData().nodes.filter(n => n.group !== 'default' && n.group !== 'bootstrap'); 
    // Shuffle or sort by connections. Let's just pick top connected nodes
    tourNodes.sort((a,b) => {
        let countA = processedEdges.filter(e => e.to === a.id || e.from === a.id).length;
        let countB = processedEdges.filter(e => e.to === b.id || e.from === b.id).length;
        return countB - countA;
    });

    const steps = tourNodes.slice(0, 10); // Visit top 10 important nodes
    if (steps.length === 0) return alert("No nodes to tour!");

    nodeInfo.innerHTML = `<h3 style="color:#4facf7;">🎬 Tour Started!</h3><p>Flying through the architecture...</p><button class="ide-btn" style="background-color:#444;" onclick="stopTour()">Stop Tour</button>`;

    steps.forEach((node, index) => {
        const t = setTimeout(() => {
            if(!tourActive) return;
            flyTo3DNode(node);
            nodeInfo.innerHTML = `<h3 style="color:#4facf7;">🎬 Tour in Progress</h3>
                                  <p><strong>Current Stop:</strong> ${node.label}</p>
                                  <p><strong>Module:</strong> ${node.group}</p>
                                  <button class="ide-btn" style="background-color:#444;" onclick="stopTour()">Stop Tour</button>`;
            
            // End of tour
            if (index === steps.length - 1) {
                setTimeout(() => {
                    if(tourActive) nodeInfo.innerHTML = `<h3 style="color:#27ae60;">✅ Tour Finished!</h3>`;
                    tourActive = false;
                }, 4000);
            }
        }, index * 4000); // 4 seconds per stop
        tourTimeouts.push(t);
    });
}

window.stopTour = function() {
    tourActive = false;
    tourTimeouts.forEach(clearTimeout);
    tourTimeouts = [];
}

// ----------------- Events -----------------
document.getElementById('refreshBtn').addEventListener('click', loadGraph);
document.getElementById('tourBtn').addEventListener('click', playTour);

document.getElementById('viewModeSelect').addEventListener('change', (e) => {
    switchViewMode(e.target.value);
});

document.getElementById('roleSelect').addEventListener('change', (e) => {
    currentRole = e.target.value;
    applyRoleAndProcessData();
    applyColorMode();
});

document.getElementById('colorBySelect').addEventListener('change', (e) => {
    currentColorMode = e.target.value;
    applyColorMode();
});

function applyColorMode() {
    if (currentMode === '3d-force' && graph3D) {
        if (currentColorMode === 'heatmap') {
            graph3D.nodeVal(node => Math.max((node.complexity || 5) / 2, 2));
        } else {
            graph3D.nodeVal(5);
        }
        graph3D.nodeColor(node => {
            if (currentColorMode === 'heatmap') return getHeatmapColor(node.complexity);
            return node.color;
        });
    }
    resetFocus(); // Tái tạo lại màu sắc cho 2D
    updateLegend();
}

function updateLegend() {
    const legendContent = document.getElementById('legend-content');
    if (!legendContent) return;

    let html = '';
    if (currentColorMode === 'heatmap') {
        html += `<div style="margin-bottom: 15px; color: #aaa;">Colors based on class complexity (methods + fields + dependencies).</div>`;
        html += `<div class="legend-item"><div class="legend-color" style="background-color: ${getHeatmapColor(0)};"></div> Low Complexity (0-10)</div>`;
        html += `<div class="legend-item"><div class="legend-color" style="background-color: ${getHeatmapColor(15)};"></div> Medium Complexity (~15)</div>`;
        html += `<div class="legend-item"><div class="legend-color" style="background-color: ${getHeatmapColor(25)};"></div> High Complexity (~25)</div>`;
        html += `<div class="legend-item"><div class="legend-color" style="background-color: ${getHeatmapColor(30)}; box-shadow: 0 0 5px red;"></div> Danger Zone (30+)</div>`;
        
        // Add downstream impact legend
        html += `<hr style="border-color:#444; margin: 15px 0;">`;
        html += `<div style="margin-bottom: 10px; color: #aaa;">Downstream Impact</div>`;
        html += `<div class="legend-item"><div class="legend-color" style="background-color: #e74c3c;"></div> Root Node</div>`;
        html += `<div class="legend-item"><div class="legend-color" style="background-color: #e67e22;"></div> Affected Node</div>`;
    } else {
        html += `<div style="margin-bottom: 15px; color: #aaa;">Colors based on feature groups.</div>`;
        const groups = new Set();
        processedNodes.forEach(n => {
            if (n.group) groups.add(n.group);
        });
        
        Array.from(groups).sort().forEach(g => {
            html += `<div class="legend-item"><div class="legend-color" style="background-color: ${getGroupColor(g)};"></div> ${g}</div>`;
        });
    }
    legendContent.innerHTML = html;
}

function getHeatmapColor(complexity) {
    if (!complexity) return '#27ae60'; // Green
    const ratio = Math.min(complexity / 30, 1.0); // Ngưỡng nguy hiểm là 30
    const hue = (1.0 - ratio) * 120; // 120 is Green, 0 is Red
    return `hsl(${hue}, 80%, 50%)`;
}

document.getElementById('searchBtn').addEventListener('click', () => {
    const query = document.getElementById('searchInput').value.toLowerCase();
    if (!query) return resetFocus();

    const matchedNode = processedNodes.find(n => n.label.toLowerCase().includes(query) || n.id.toLowerCase().includes(query));
    if (matchedNode) {
        if(currentMode !== '3d-force') {
            network2D.selectNodes([matchedNode.id]);
            focusOnNode2D(matchedNode.id);
            network2D.focus(matchedNode.id, { scale: 1.2, animation: true });
        } else {
            const node3D = graph3D.graphData().nodes.find(n => n.id === matchedNode.id);
            if(node3D) {
                flyTo3DNode(node3D);
                displayNodeInfo(node3D);
            }
        }
    } else {
        alert("No node found matching: " + query);
    }
});

document.getElementById('searchInput').addEventListener('keypress', (e) => {
    if (e.key === 'Enter') document.getElementById('searchBtn').click();
});

// Cài đặt Right-Click Pan cho 2D
container2D.addEventListener('contextmenu', e => e.preventDefault());
let isRightDragging = false;
let lastPanPos = {x:0, y:0};

container2D.addEventListener('mousedown', e => {
    if (e.button === 2) {
        isRightDragging = true;
        lastPanPos = {x: e.clientX, y: e.clientY};
        container2D.style.cursor = 'grabbing';
    }
});

container2D.addEventListener('mousemove', e => {
    if (isRightDragging && network2D) {
        const dx = e.clientX - lastPanPos.x;
        const dy = e.clientY - lastPanPos.y;
        lastPanPos = {x: e.clientX, y: e.clientY};
        
        const currentView = network2D.getViewPosition();
        const scale = network2D.getScale();
        network2D.moveTo({
            position: { x: currentView.x - dx/scale, y: currentView.y - dy/scale },
            animation: false
        });
    }
});

window.addEventListener('mouseup', e => {
    if (e.button === 2) {
        isRightDragging = false;
        container2D.style.cursor = 'default';
    }
});

// Chạy lần đầu khi DOM đã tải xong
window.addEventListener('load', loadGraph);

// Resize observer
window.addEventListener('resize', () => {
    const container = document.getElementById('graph-container');
    if(currentMode === '3d-force' && graph3D) {
        graph3D.width(container.clientWidth).height(container.clientHeight);
    } else if (network2D) {
        network2D.setSize(container.clientWidth + "px", container.clientHeight + "px");
        network2D.redraw();
    }
});

// ==========================================
// UNITY 3D CAMERA CONTROLS (WASD + Right Click Look)
// ==========================================
const keys = { w: false, a: false, s: false, d: false, q: false, e: false };
window.addEventListener('keydown', e => {
    const k = e.key.toLowerCase();
    if (keys.hasOwnProperty(k)) keys[k] = true;
});
window.addEventListener('keyup', e => {
    const k = e.key.toLowerCase();
    if (keys.hasOwnProperty(k)) keys[k] = false;
});

let originalTargetDist = 100;
document.getElementById('graph-container').addEventListener('mousedown', e => {
    if (e.button === 2 && currentMode === '3d-force' && graph3D) {
        const cam = graph3D.camera();
        const controls = graph3D.controls();
        
        let fx = controls.target.x - cam.position.x;
        let fy = controls.target.y - cam.position.y;
        let fz = controls.target.z - cam.position.z;
        originalTargetDist = Math.sqrt(fx*fx + fy*fy + fz*fz) || 100;
        
        fx /= originalTargetDist; fy /= originalTargetDist; fz /= originalTargetDist;
        controls.target.set(cam.position.x + fx * 1, cam.position.y + fy * 1, cam.position.z + fz * 1);
        controls.update();
    }
});

window.addEventListener('mouseup', e => {
    if (e.button === 2 && currentMode === '3d-force' && graph3D) {
        const cam = graph3D.camera();
        const controls = graph3D.controls();
        
        let fx = controls.target.x - cam.position.x;
        let fy = controls.target.y - cam.position.y;
        let fz = controls.target.z - cam.position.z;
        const lenF = Math.sqrt(fx*fx + fy*fy + fz*fz) || 1;
        fx /= lenF; fy /= lenF; fz /= lenF;
        
        controls.target.set(cam.position.x + fx * originalTargetDist, cam.position.y + fy * originalTargetDist, cam.position.z + fz * originalTargetDist);
        controls.update();
    }
});

function wasdLoop() {
    if (currentMode === '3d-force' && graph3D) {
        if (keys.w || keys.s || keys.a || keys.d || keys.q || keys.e) {
            const cam = graph3D.camera();
            const controls = graph3D.controls();
            let dx = 0, dy = 0, dz = 0;
            const speed = 5.0; // Tốc độ bay
            
            let fx = controls.target.x - cam.position.x;
            let fy = controls.target.y - cam.position.y;
            let fz = controls.target.z - cam.position.z;
            const lenF = Math.sqrt(fx*fx + fy*fy + fz*fz) || 1;
            fx /= lenF; fy /= lenF; fz /= lenF;
            
            let rx = -fz; let ry = 0; let rz = fx;
            const lenR = Math.sqrt(rx*rx + rz*rz) || 1;
            rx /= lenR; rz /= lenR;
            
            if (keys.w) { dx += fx*speed; dy += fy*speed; dz += fz*speed; }
            if (keys.s) { dx -= fx*speed; dy -= fy*speed; dz -= fz*speed; }
            if (keys.a) { dx -= rx*speed; dy -= ry*speed; dz -= rz*speed; }
            if (keys.d) { dx += rx*speed; dy += ry*speed; dz += rz*speed; }
            if (keys.q) { dy -= speed; } // Bay xuống
            if (keys.e) { dy += speed; } // Bay lên
            
            cam.position.set(cam.position.x + dx, cam.position.y + dy, cam.position.z + dz);
            controls.target.set(controls.target.x + dx, controls.target.y + dy, controls.target.z + dz);
            controls.update();
        }
    }
    requestAnimationFrame(wasdLoop);
}
wasdLoop();
