    using UnityEngine;
    public enum ObstacleType
    {
        Jump,
        Slide
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
        bool isSpecial = false;
        SpecialType specialType;
        
        

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
            if (r < 0.7f) return ObstacleType.Jump;
            else return ObstacleType.Slide;
        }

        void Start()
        {
            if (isFirstChunk) return;

            if (isSpecial)
                GenerateSpecialPattern();
            else
                GenerateObstaclePattern();
        }

        void GenerateObstaclePattern()
        {
            int pattern = difficulty.GetPattern(); // ตัดสินใจ pattern จาก difficulty

            switch (pattern)
            {
                case 0:
                    SpawnOneLane();
                    break;
                case 1:
                    SpawnTwoLanes();
                    break;
                case 2:
                    // ไม่มี obstacle ใน chunk นี้
                    break;
            }
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

        void SpawnOneLane()
        {
            int lane = Random.Range(0, 3);
            var type = RollObstacleType();
            SpawnType(type, lanePoints[lane]);
        }

        void SpawnTwoLanes()
        {
            int a = Random.Range(0, 3);
            int b;
            do
            {
                b = Random.Range(0, 3);
            } while (b == a);

            var tA = RollObstacleType();
            var tB = RollObstacleType();

            SpawnType(tA, lanePoints[a]);
            SpawnType(tB, lanePoints[b]);
        }
        void SpawnType(ObstacleType type, Transform lane)
        {
            switch(type)
            {
                case ObstacleType.Jump:
                    factory.SpawnBlock(lane);
                    break;
                case ObstacleType.Slide:
                    factory.SpawnLowBlock(lane);
                    break;
            }
        }
    }
