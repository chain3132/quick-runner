    using MoreMountains.Feedbacks;
    using UnityEngine;
    using UnityEngine.PlayerLoop;

    public enum ObstacleType
    {
        Jump,
        Slide,
        ForcedDodge
    }
    public enum SpecialType
    {
        BreakWall,
        ShrinkTunnel,
        LongGap
    }
    public class Chunk : MonoBehaviour
    {
        public Transform[] lanePoints; // lane0, lane1, lane2
        public ObstacleFactory factory;
        public DifficultyController difficulty;
        public bool isFirstChunk;
        private bool isSetSafe = false;
        
        bool isSpecial = false;
        SpecialType specialType;

        public void SetSafe()
        {
            isSetSafe = true;
        }
        public bool IsForcedDodge(ObstacleType type)
        {
            return type == ObstacleType.ForcedDodge;
        }
        public void SetSpecial(SpecialType t)
        {
            isSpecial = true;
            specialType = t;
        }

        public void SetNormal()
        {
            isSpecial = false;
        }
        
        public ObstacleType RollObstacleType()
        {
            float r = Random.value;
            if (r < 0.6f) return ObstacleType.Jump;
            if (r < 0.85f) return ObstacleType.Slide;
            return ObstacleType.ForcedDodge;
        }

        void Start()
        {
            if (isFirstChunk) return;
            if (isSetSafe) return;
            if (isSpecial)
                GenerateSpecialPattern();
            else
                GenerateObstaclePattern();
        }

        void GenerateObstaclePattern()
        {
            int slotCount = Random.Range(2, 3); // จำนวน obstacle 1–2ต่อ chunk
            float minSpacing = 6f;
    
            float[] zSlots = GenerateSlotLayout(slotCount, minSpacing, chunkLength: 12.5f);

            for (int i = 0; i < zSlots.Length; i++)
            {
                SpawnSlot(zSlots[i]);
            }
        }
        float[] GenerateSlotLayout(int count, float minSpacing, float chunkLength)
        {
            System.Collections.Generic.List<float> slots = new System.Collections.Generic.List<float>();

            int attempts = 0;
            while (slots.Count < count && attempts < 25)
            {
                attempts++;
                float z = Random.Range(2f, chunkLength - 2f);

                bool valid = true;
                foreach (var s in slots)
                {
                    if (Mathf.Abs(s - z) < minSpacing)
                    {
                        valid = false;
                        break;
                    }
                }

                if (valid)
                {
                    slots.Add(z);
                }
            }

            slots.Sort();
            return slots.ToArray();
        }
        void SpawnSlot(float z)
        {
            ObstacleType type = RollObstacleType();

            if (IsForcedDodge(type))
            {
                SpawnForcedDodgePattern(type, z);
                return;
            }

            float r = Random.value;

            if (r < 0.7f)
                SpawnSingleLane(z, type);
            else if (r < 0.9f)
                SpawnTwoLanes(z, type);
            else
                SpawnAllLanes(z, type);
        }
        void SpawnForcedDodgePattern(ObstacleType type, float z)
        {
            int safeLane = Random.Range(0, 3);

            for (int lane = 0; lane < 3; lane++)
            {
                if (lane == safeLane) continue;
                factory.SpawnTypeAt(type, lanePoints[lane], z);
            }
        }
        void SpawnSingleLane(float z, ObstacleType type)
        {
            int lane = Random.Range(0, 3);
            factory.SpawnTypeAt(type, lanePoints[lane], z);
        }

        void SpawnTwoLanes(float z, ObstacleType type)
        {
            int a = Random.Range(0, 3);
            int b;
            do { b = Random.Range(0, 3); } while (b == a);

            factory.SpawnTypeAt(type, lanePoints[a], z);
            factory.SpawnTypeAt(type, lanePoints[b], z);
        }

        void SpawnAllLanes(float z, ObstacleType type)
        {
            for (int lane = 0; lane < 3; lane++)
                factory.SpawnTypeAt(type, lanePoints[lane], z);
        }
        
        void GenerateSpecialPattern()
        {
            switch(specialType)
            {
                case SpecialType.BreakWall:
                    SpawnBreakWall();
                    break;
                
                case SpecialType.ShrinkTunnel:
                    SpawnShrinkTunnel();
                    break;
            }
        }
        void SpawnBreakWall()
        {
            factory.SpawnBreakWall(lanePoints[1]); 
        }

        void SpawnLongGap()
        {
            factory.SpawnLongGap(lanePoints[1]); 
        }

        void SpawnShrinkTunnel()
        {
            factory.SpawnTunnel(lanePoints[1]);
        }

        

        
        
    }
