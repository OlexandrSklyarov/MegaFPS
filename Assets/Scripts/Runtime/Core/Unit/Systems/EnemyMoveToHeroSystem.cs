using Leopotam.EcsLite;
using Runtime.Extensions;
using UnityEngine;

namespace SA.FPS
{
    public sealed class EnemyMoveToHeroSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _enemyFilter;
        private EcsFilter _heroFilter;
        private EcsPool<NavigationComponent> _navPool;
        private EcsPool<UnitViewComponent> _enemyViewPool;
        private EcsPool<HeroComponent> _heroPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _enemyFilter = _world
                .Filter<EnemyUnitTag>()
                .Inc<UnitViewComponent>()
                .Inc<NavigationComponent>()
                .Exc<EnemyAttackComponent>()
                .End(); 

            _heroFilter = _world.Filter<HeroComponent>().End();             

            _navPool = _world.GetPool<NavigationComponent>();
            _enemyViewPool = _world.GetPool<UnitViewComponent>();
            _heroPool = _world.GetPool<HeroComponent>();
        }


        public void Run(IEcsSystems systems)
        {           
            foreach(var heroEnt in _heroFilter)
            {
                ref var hero = ref _heroPool.Get(heroEnt); 
                var heroPos = hero.ViewRef.transform.position;

                foreach(var enemyEnt in _enemyFilter)
                {
                    ref var navigation = ref _navPool.Get(enemyEnt); 
                    ref var view = ref _enemyViewPool.Get(enemyEnt); 

                    if (navigation.StopDelay > 0f)
                    {
                        navigation.StopDelay -= Time.deltaTime;
                        continue;
                    }

                    if (navigation.NextUpdateDestinationTimer > 0f)
                    {
                        navigation.NextUpdateDestinationTimer -= Time.deltaTime;
                        continue;
                    }

                    var config = view.ViewRef.Config;           
                    var isTargetFar = IsTargetFar(ref navigation, ref view, heroPos);     
                    var targetSpeed =  (isTargetFar) ? navigation.Speed : 0f;

                    navigation.AgentRef.enabled = true;
                    navigation.AgentRef.speed = targetSpeed;
                    navigation.AgentRef.SetDestination(heroPos);
                    navigation.AgentRef.stoppingDistance = view.ViewRef.Config.AttackRange;
                    navigation.NextUpdateDestinationTimer = config.UpdateDestinationDelay;

                    if (!isTargetFar) 
                    {
                        ref var attack = ref _world.GetOrAddComponent<EnemyAttackComponent>(enemyEnt);
                        attack.AttackIndex  = UnityEngine.Random.Range(0, view.ViewRef.Config.AttackTimeCollection.Length);
                        attack.TargetPos = heroPos;
                    }
                }
            }
        }


        private bool IsTargetFar(ref NavigationComponent navigation, ref UnitViewComponent view, Vector3 heroPos)
        {
            return (navigation.AgentRef.transform.position - heroPos).sqrMagnitude > 
                view.ViewRef.Config.AttackRange * view.ViewRef.Config.AttackRange;
        }
    }
}