using NUnit.Framework;

namespace DataStructures.Tests
{
    [TestFixture]
    public class W3Tests : BaseTests
    {
        const string path = @"W3 - Hash Tables\";
        const string location_phonebook = path + "1 phone_book";
        const string location_hash_chains = path + "2 hash_chains";
        const string location_hash_substring = path + "3 hash_substring";

        [Test]
        public void PhonebookTests()
        {
            //TestDirectory(location_phonebook, Phonebook.Answer);
        }

        [Test]
        public void HashChainsTests()
        {
            //TestDirectory(location_hash_chains, HashChains.Answer);
        }

        [Test]
        public void HashSubstringTests()
        {
            //TestDirectory(location_hash_substring, HashSubstring.Answer);
        }
    }
}
