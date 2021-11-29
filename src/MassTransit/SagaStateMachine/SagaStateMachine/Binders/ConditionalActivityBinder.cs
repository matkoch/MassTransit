namespace MassTransit.SagaStateMachine
{
    using System.Threading.Tasks;


    public class ConditionalActivityBinder<TSaga> :
        IActivityBinder<TSaga>
        where TSaga : class, ISaga
    {
        readonly StateMachineAsyncCondition<TSaga> _condition;
        readonly EventActivities<TSaga> _elseActivities;
        readonly Event _event;
        readonly EventActivities<TSaga> _thenActivities;

        public ConditionalActivityBinder(Event @event, StateMachineCondition<TSaga> condition,
            EventActivities<TSaga> thenActivities, EventActivities<TSaga> elseActivities)
            : this(@event, context => Task.FromResult(condition(context)), thenActivities, elseActivities)
        {
        }

        public ConditionalActivityBinder(Event @event, StateMachineAsyncCondition<TSaga> condition,
            EventActivities<TSaga> thenActivities, EventActivities<TSaga> elseActivities)
        {
            _thenActivities = thenActivities;
            _elseActivities = elseActivities;
            _condition = condition;
            _event = @event;
        }

        public bool IsStateTransitionEvent(State state)
        {
            return Equals(_event, state.Enter) || Equals(_event, state.BeforeEnter)
                || Equals(_event, state.AfterLeave) || Equals(_event, state.Leave);
        }

        public void Bind(State<TSaga> state)
        {
            IBehavior<TSaga> thenBehavior = GetBehavior(_thenActivities);
            IBehavior<TSaga> elseBehavior = GetBehavior(_elseActivities);

            var conditionActivity = new ConditionActivity<TSaga>(_condition, thenBehavior, elseBehavior);

            state.Bind(_event, conditionActivity);
        }

        public void Bind(IBehaviorBuilder<TSaga> builder)
        {
            IBehavior<TSaga> thenBehavior = GetBehavior(_thenActivities);
            IBehavior<TSaga> elseBehavior = GetBehavior(_elseActivities);

            var conditionActivity = new ConditionActivity<TSaga>(_condition, thenBehavior, elseBehavior);

            builder.Add(conditionActivity);
        }

        static IBehavior<TSaga> GetBehavior(EventActivities<TSaga> activities)
        {
            var builder = new ActivityBehaviorBuilder<TSaga>();

            foreach (IActivityBinder<TSaga> activity in activities.GetStateActivityBinders())
                activity.Bind(builder);

            return builder.Behavior;
        }
    }


    public class ConditionalActivityBinder<TSaga, TMessage> :
        IActivityBinder<TSaga>
        where TSaga : class, ISaga
        where TMessage : class
    {
        readonly StateMachineAsyncCondition<TSaga, TMessage> _condition;
        readonly EventActivities<TSaga> _elseActivities;
        readonly Event _event;
        readonly EventActivities<TSaga> _thenActivities;

        public ConditionalActivityBinder(Event @event, StateMachineCondition<TSaga, TMessage> condition,
            EventActivities<TSaga> thenActivities, EventActivities<TSaga> elseActivities)
            : this(@event, context => Task.FromResult(condition(context)), thenActivities, elseActivities)
        {
        }

        public ConditionalActivityBinder(Event @event, StateMachineAsyncCondition<TSaga, TMessage> condition,
            EventActivities<TSaga> thenActivities, EventActivities<TSaga> elseActivities)
        {
            _thenActivities = thenActivities;
            _elseActivities = elseActivities;
            _condition = condition;
            _event = @event;
        }

        public bool IsStateTransitionEvent(State state)
        {
            return Equals(_event, state.Enter) || Equals(_event, state.BeforeEnter)
                || Equals(_event, state.AfterLeave) || Equals(_event, state.Leave);
        }

        public void Bind(State<TSaga> state)
        {
            IBehavior<TSaga> thenBehavior = GetBehavior(_thenActivities);
            IBehavior<TSaga> elseBehavior = GetBehavior(_elseActivities);

            var conditionActivity = new ConditionActivity<TSaga, TMessage>(_condition, thenBehavior, elseBehavior);

            state.Bind(_event, conditionActivity);
        }

        public void Bind(IBehaviorBuilder<TSaga> builder)
        {
            IBehavior<TSaga> thenBehavior = GetBehavior(_thenActivities);
            IBehavior<TSaga> elseBehavior = GetBehavior(_elseActivities);

            var conditionActivity = new ConditionActivity<TSaga, TMessage>(_condition, thenBehavior, elseBehavior);

            builder.Add(conditionActivity);
        }

        static IBehavior<TSaga> GetBehavior(EventActivities<TSaga> activities)
        {
            var builder = new ActivityBehaviorBuilder<TSaga>();

            foreach (IActivityBinder<TSaga> activity in activities.GetStateActivityBinders())
                activity.Bind(builder);

            return builder.Behavior;
        }
    }
}
