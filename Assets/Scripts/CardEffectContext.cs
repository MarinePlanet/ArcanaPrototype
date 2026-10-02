namespace ArcanaPrototype
{
    public enum CardDestination
    {
        Discard,
        Exhaust,
        ReturnToHand,
        ShuffleIntoDrawPile
    }

    public class CardEffectContext
    {
        public CardInstance SourceCard { get; }
        public ArcanaOrientation Orientation { get; }
        public HandManager HandManager { get; }
        public DeckManager DeckManager { get; }
        public PlayerHealth PlayerHealth { get; }
        public EnemyHealth EnemyHealth { get; }
        public EnvironmentManager EnvironmentManager { get; }
        public StatusManager PlayerStatusManager { get; }
        public StatusManager EnemyStatusManager { get; }
        public CardDestination Destination { get; set; } = CardDestination.Discard;

        public CardEffectContext(
            CardInstance sourceCard,
            ArcanaOrientation orientation,
            HandManager handManager,
            DeckManager deckManager,
            PlayerHealth playerHealth,
            EnemyHealth enemyHealth,
            EnvironmentManager environmentManager,
            StatusManager playerStatusManager,
            StatusManager enemyStatusManager)
        {
            SourceCard = sourceCard;
            Orientation = orientation;
            HandManager = handManager;
            DeckManager = deckManager;
            PlayerHealth = playerHealth;
            EnemyHealth = enemyHealth;
            EnvironmentManager = environmentManager;
            PlayerStatusManager = playerStatusManager;
            EnemyStatusManager = enemyStatusManager;
        }
    }
}
