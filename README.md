# Grid-Based Incremental Game Prototype

A technical prototype exploring grid systems, graph algorithms, and electrical network simulation in Unity. Built as a learning project to understand spatial data structures, pathfinding algorithms, and shader programming.

![Power Network System](screenshots/Ekran_Görüntüsü__29_.png)
*Complete system: Grid placement, power distribution network, and resource gathering*

## 🎯 Project Goals

This prototype was built to explore:
- **Custom grid system implementation** using spatial hashing
- **Graph algorithms** applied to electrical power distribution
- **Shader programming** for visual effects
- **Multi-tile building placement** with validation
- **Interface-driven architecture** for modular systems

## 🔧 What I Built

### 1. Custom Grid System
Built a grid system from scratch using Dictionary-based spatial hashing instead of 2D arrays.

**Key Learning:**
- Spatial hashing for O(1) grid queries
- Multi-tile object placement validation
- Grid coordinate mapping and neighbor detection

```csharp
Dictionary<Vector3Int, GridSpec> grid
```

**Why Dictionary over Array?**
- Flexible grid size (no pre-allocation needed)
- Fast lookups without iterating through empty cells
- Easy to extend to 3D or irregular grids

![Clean Grid View](screenshots/Ekran_Görüntüsü__24_.png)
*Grid system with resource deposits and unpowered electrical pole (red)*

### 2. Electrical Power Network (BFS Algorithm)

Implemented a power distribution system using **Breadth-First Search** to traverse electrical connections.

**How it works:**
1. When a power pole is placed/connected, trigger BFS search
2. Traverse all connected electrical nodes
3. Check if any generator exists in the network
4. Update visual state (green = powered, red = no power)

```csharp
// BFS implementation for power network
Queue<IElectricNode> queue = new Queue<IElectricNode>();
HashSet<IElectricNode> visited = new HashSet<IElectricNode>();
```

**Technical Insights:**
- Graph theory applied to game systems
- BFS for connected component detection
- Real-time network recalculation on topology changes

![Power Connection](screenshots/Ekran_Görüntüsü__25_.png)
*Powered pole (green) connected to generator via cable*

![Network Expansion](screenshots/Ekran_Görüntüsü__26_.png)
*Disconnected network section - red poles indicate no power source*

### 3. Custom Rope/Cable System

Modified a third-party rope package to create dynamic electrical cables:
- **Vertex painting** to mark cable segments
- **Custom Shader Graph** reading vertex colors for wave animation
- Dynamic material swapping (powered = bright, unpowered = dim)

**Shader Challenge:**
Creating smooth wave effects using vertex color data instead of UV coordinates required understanding how Unity's mesh pipeline works.

![Complete Network](screenshots/Ekran_Görüntüsü__28_.png)
*Full power network with BFS connecting all nodes to the generator*

### 4. Interface-Driven Architecture

Designed flexible component system using interfaces:

```csharp
IPlaceable    // Objects that can be placed on grid
IPickable     // Objects that can be picked up
IElectricNode // Components in the power network
IGenerator    // Power-producing structures
```

**Benefits:**
- Easy to add new building types
- Components can implement multiple behaviors
- Clear separation of concerns

## 📸 Visual Progression

### 1. Initial Grid Setup
![Grid Setup](screenshots/Ekran_Görüntüsü__24_.png)
*Base grid with resource deposits and an unpowered pole*

### 2. First Power Connection
![First Connection](screenshots/Ekran_Görüntüsü__25_.png)
*Pole turns green when connected to generator - BFS validates power source*

### 3. Isolated Network
![Isolated Network](screenshots/Ekran_Görüntüsü__26_.png)
*Adding poles without generator connection - remains red (no power)*

### 4. Multi-Tile Building
![Building Placement](screenshots/Ekran_Görüntüsü__28_.png)
*2x2 generator building with grid validation system*

### 5. Complete Power Grid
![Full System](screenshots/Ekran_Görüntüsü__29_.png)
*BFS algorithm connects all nodes, cables show power flow with custom shader*

## 💡 What I Learned

### Data Structures
- **Dictionary vs Array** trade-offs for spatial data
- **HashSet** for efficient visited tracking in BFS
- **Queue** for level-order graph traversal

### Algorithms
- Breadth-First Search for graph traversal
- Spatial hashing for position-based lookups
- Connected component detection in graphs

### Unity Systems
- **Vertex painting** workflow in Unity
- **Shader Graph** for procedural effects
- **Unity Splines** for cable mesh generation
- **New Input System** for player controls

### Design Patterns
- Interface segregation principle
- Event-driven architecture (OnGridChanged, OnPowerChanged)
- Component-based design

## 🛠️ Technical Stack

- **Unity 2022.3+** (URP)
- **C#** for all game logic
- **Unity Splines** for cable rendering
- **Shader Graph** for custom shaders
- **Modified third-party rope asset** with custom vertex painting

## 📂 Code Structure

```
Assets/_Scripts/
├── Grid/           # Custom grid system
│   ├── GridSystem.cs
│   └── IPlaceable.cs, IPickable.cs
├── Electric/       # Power network & BFS
│   ├── PowerManager.cs (BFS implementation)
│   ├── ElectricPipes.cs
│   └── Generator.cs
├── Player/         # Placement & interaction
└── Deposits/       # Resource gathering
```

## 🎓 Key Takeaways

This project taught me that:
1. **Sometimes building from scratch is better** - I learned more by implementing a custom grid system than I would have using Unity's Tilemap
2. **Algorithms aren't just for interviews** - BFS perfectly solved the power network problem
3. **Shader programming is powerful** - Small shader tweaks create big visual impact
4. **Interfaces enable flexibility** - Interface-based design made adding new features easy

## ⚠️ Known Limitations

This is a **prototype**, not a finished game:
- No gameplay loop or win condition
- Limited building variety
- No save/load system
- Performance not optimized for large grids

The goal was to learn core systems, not build a complete game.

---

**Developer**: Mert Özzencir  
**GitHub**: [MertOzzencir](https://github.com/MertOzzencir)  
**Focus**: Learning grid systems, algorithms, and shader programming
