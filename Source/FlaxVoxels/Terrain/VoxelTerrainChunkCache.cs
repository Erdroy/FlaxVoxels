// FlaxVoxels (c) 2018 Damian 'Erdroy' Korczowski

using System;
using System.Collections.Generic;
using System.Linq;

namespace FlaxVoxels.Terrain
{
	public class VoxelTerrainChunkCache
	{
	    private struct CacheEntry
	    {
	        public VoxelTerrainChunk Chunk { get; set; }
	        public DateTime CacheTime { get; set; }
	    }

	    private readonly List<CacheEntry> _cache;

	    internal VoxelTerrainChunkCache()
	    {
	        _cache = new List<CacheEntry>();
        }

	    public void Update()
	    {
	        lock (_cache)
	        {
	            if (_cache.Count == 0)
	                return;

	            var currentTime = DateTime.Now;

	            for (var i = 0; i < _cache.Count; i++)
	            {
                    // TODO: Destroy chunks that are cached long enough
	            }
	        }
        }

	    public void Add(VoxelTerrainChunk chunk)
	    {
	        lock (_cache)
	        {
	            _cache.Add(new CacheEntry
	            {
                    CacheTime = DateTime.Now,
                    Chunk = chunk
                });
            }
        }

	    public bool Contains(VoxelTerrainChunk chunk)
        {
            lock (_cache)
            {
                return _cache.Count(x => x.Chunk == chunk) != 0;
            }
        }

	    public void Remove(VoxelTerrainChunk chunk)
	    {
	        lock (_cache)
	        {
	            var entry = _cache.FirstOrDefault(x => x.Chunk == chunk);
	            _cache.Remove(entry);
	        }
        }
	}
}
