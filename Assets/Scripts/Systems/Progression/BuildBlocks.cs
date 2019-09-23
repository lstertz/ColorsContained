using Components.Activity;
using Entities.Environment;
using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Systems.Progression
{
	/// <summary>
	/// Defines the Build Blocks System, which advances all ToBeBuilt Blocks 
    /// to Built Blocks.
	/// </summary>
    [AlwaysUpdateSystem]
    public class BuildBlocks : ComponentSystem
    {
        public static bool AllBlocksBuilt { get; private set; } = false;
        private List<Tuple<float3, Color>> blockDataToConstruct = new List<Tuple<float3, Color>>();

        protected override void OnCreateManager()
        {
            Enabled = false;
        }

        [BurstCompile]
        protected override void OnUpdate()
        {
            AllBlocksBuilt = false;


            blockDataToConstruct.Clear();
            int highestActivationCount = 0;
            Entities.ForEach((ref TileProperties properties, ref Position position) =>
            {
                if (properties.ActivationCount > highestActivationCount)
                    highestActivationCount = properties.ActivationCount;
            });

            if (highestActivationCount == 0)
            {
                AllBlocksBuilt = true;
                return;
            }

            Entities.ForEach((ref BlockProperties properties, ref Position position) =>
            {
                position.Value.y++;
            });

            Entities.ForEach((ref TileProperties properties, ref Position position) =>
            {
                if (properties.ActivationCount != highestActivationCount)
                    return;

                float3 pos = new float3(position.Value.x, position.Value.y + 1, position.Value.z);
                blockDataToConstruct.Add(new Tuple<float3, Color>(
                    pos, properties.ActivatedColor));
                properties.ActivationCount--;
            });


            for (int c = 0, count = blockDataToConstruct.Count; c < count; c++)
                Block.Create(blockDataToConstruct[c].Item1, blockDataToConstruct[c].Item2);
        }
    }
}
