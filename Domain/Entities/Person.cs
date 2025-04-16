namespace Domain.Entities
{
    public class Person
    {
        private readonly Dictionary<string, State> _stateMappings = new Dictionary<string, State>
        {
            {"home", State.Home},
            {"unknown", State.Unknown},
            {"not_home", State.NotHome},
            {"unavailable", State.Unavailable}
        };

        public enum State
        {
            Unknown,
            Unavailable,
            Home,
            NotHome
        }

        public Person(string? personsState)
        {
            if (!_stateMappings.TryGetValue(personsState ?? string.Empty, out var currentState))
            {
                // zone_name – If the person is detected in a defined zone, their state will 
                // be the name of that zone (e.g., work, gym, school).
                currentState = State.NotHome;
            }

            CurrentState = currentState;
        }

        public State CurrentState { get; private set; }
        
        public bool IsHome => CurrentState == State.Home;
    }
}