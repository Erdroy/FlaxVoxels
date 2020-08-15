// FlaxVoxels (c) 2018-2019 Damian 'Erdroy' Korczowski

using System.Collections.Concurrent;
using System.Threading;
using FlaxEngine;
using FlaxVoxels.Terrain;
using FlaxVoxels.Terrain.Generator;
using FlaxVoxels.Terrain.Meshing;

namespace FlaxVoxels
{
    /// <summary>
    ///     VoxelTerrainChunkGenerator class. Provides multi-threaded chunk generation.
    /// </summary>
	public class VoxelTerrainChunkGenerator : Script
    {
        private struct GeneratorTask
        {
            public bool GenerateVoxels { get; set; }
            public bool GenerateMesh { get; set; }

            public VoxelTerrainChunk Chunk { get; set; }
        }

        private volatile bool _isRunning;
        private ConcurrentQueue<GeneratorTask> _tasks;
        private Thread[] _threads;

        public override void OnStart()
        {
            base.OnStart();

            Current = this;
            _tasks = new ConcurrentQueue<GeneratorTask>();

            // Start threads
            SetupThreads();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            // TODO: 
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            KillThreads();
        }

        private void WorkerProcessOne(GeneratorTask task, IVoxelTerrainGenerator generator, IVoxelTerrainMesher mesher)
        {
            var chunk = task.Chunk;

            // Chunk generation rules/problems:
            // - To generate chunk mesh, the chunk must have all neighbor chunks
            // - Neighbor chunk can be still generating (how to deal with this?)
            // - We cannot make the chunk active (chunk.IsActive=true), until meshing finishes

            // TODO: Chunk processing implementation

            // Temporary
            if (task.GenerateVoxels)
            {
                chunk.State = VoxelTerrainChunkState.GeneratingVoxels;
                chunk.WorkerGenerateVoxels(generator);
            }

            if (task.GenerateMesh)
            {
                chunk.State = VoxelTerrainChunkState.GeneratingMesh;
                chunk.WorkerGenerateMesh(mesher);
            }
        }

        private void WorkerFunction(int threadId)
        {
            Debug.Log(string.Format(this + " WorkerThread {0} (managed id: {1}) started", threadId, Thread.CurrentThread.ManagedThreadId), this);

            var currentGenerator = new DefaultVoxelGenerator();
            var currentMesher = new ErdroysCubeMesher();

            while (_isRunning)
            {
                if (!_tasks.TryDequeue(out var task))
                {
                    Thread.Sleep(ThreadWaitTime);
                    continue;
                }
                
                // Process one chunk
                WorkerProcessOne(task, currentGenerator, currentMesher);

                // Oke, we are done.
                task.Chunk.State = VoxelTerrainChunkState.Complete;
            }

            Debug.Log(string.Format(this + " WorkerThread {0} (managed id: {1}) stopped", threadId, Thread.CurrentThread.ManagedThreadId), this);
        }

        private void SetupThreads()
        {
            _isRunning = true;
            _threads = new Thread[MaxThreads];

            // Start threads
            for (var i = 0; i < MaxThreads; i++)
            {
                var threadId = i;
                var thread = _threads[i] = new Thread(() => WorkerFunction(threadId))
                {
                    Priority = DefaultThreadPriority, // Set thread priority
                    IsBackground = true // Set thread to run in background
                };
                thread.Start();
            }
        }

        private void KillThreads()
        {
            _isRunning = false;

            foreach (var thread in _threads)
            {
                // Join to thread and wait until it finished
                thread.Join();
            }

            _threads = null;
        }
        
        /// <summary>
        ///     Enqueue chunk for voxel generation and meshing.
        /// </summary>
        /// <param name="chunk">The chunk instance.</param>
        public static void EnqueueGeneration(VoxelTerrainChunk chunk)
        {
            // TODO: Deal with neighbors

            chunk.State = VoxelTerrainChunkState.QueuedForGeneration;
            Current._tasks.Enqueue(new GeneratorTask
            {
                Chunk = chunk,
                GenerateVoxels = true,
                GenerateMesh = true
            });
        }

        /// <summary>
        ///     Enqueue chunk for voxel generation.
        /// </summary>
        /// <param name="chunk">The chunk instance.</param>
        public static void EnqueueVoxelGeneration(VoxelTerrainChunk chunk)
        {
            chunk.State = VoxelTerrainChunkState.QueuedForVoxelGeneration;
            Current._tasks.Enqueue(new GeneratorTask
            {
                Chunk = chunk,
                GenerateVoxels = true,
                GenerateMesh = false
            });
        }

        /// <summary>
        ///     Enqueue chunk for meshing.
        /// </summary>
        /// <param name="chunk">The chunk instance.</param>
        public static void EnqueueMeshGeneration(VoxelTerrainChunk chunk)
        {
            // TODO: Deal with neighbors

            chunk.State = VoxelTerrainChunkState.GeneratingMesh;
            Current._tasks.Enqueue(new GeneratorTask
            {
                Chunk = chunk,
                GenerateVoxels = false,
                GenerateMesh = true
            });
        }

        /// <summary>
        ///     The maximal amount of threads that can be assigned for the generator.
        /// </summary>
        public int MaxThreads { get; set; } = 4;

        /// <summary>
        ///     The time that thread will wait when there is no any new tasks, 
        ///     after this time the thread will be trying to dequeue next task.
        /// </summary>
        public int ThreadWaitTime { get; set; } = 10;

        /// <summary>
        ///     The generator thread priority. Default is BelowNormal.
        /// </summary>
        public ThreadPriority DefaultThreadPriority { get; set; } = ThreadPriority.BelowNormal;

        /// <summary>
        ///     The current active voxel terrain chunk generator instance.
        /// </summary>
        public static VoxelTerrainChunkGenerator Current { get; private set; }
    }
}
