namespace DiceRoller.Resources
{
    public class Strings
    {
        private static Configuration configuration = new Configuration();
        private static IconUri iconUri = new IconUri();
        private static Text text = new Text();

        public Configuration Configuration
        {
            get { return configuration; }
        }

        public IconUri IconUri
        {
            get { return iconUri; }
        }

        public Text Text
        {
            get { return text; }
        }
    }
}
