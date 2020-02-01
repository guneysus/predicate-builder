namespace predicate_builder_tests
{
    internal class TestData<T>
    {
        public bool Expected { get; set; }
        public string Command { get; set; }
        public T Item { get; set; }

        public static TestData<T> New(string command, T item, bool expected)
        {
            return new TestData<T>()
            {
                Command = command,
                Item = item,
                Expected = expected
            };
        }
    }
}
