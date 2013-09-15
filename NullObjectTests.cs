using NUnit.Framework;

namespace NullObject
{
    [TestFixture]
    public class NullObjectTests
    {
        // WHAT?
        // The null object pattern replaces a potentially null object with an object that implements the 
        // right interface but has some sort of neutral behaviour.
        // An object that implements the null object pattern does not have a default implementation, but 
        // instead does nothing so you can be fairly confident it won't have side-effects.
        // It is a special case of the strategy pattern. 

        // WHY? 
        // Null reference checks can add noise to code and extra paths that need testing. Sometimes the fact
        // that the object is not there is not important to the code that is using it. 

        [Test]
        public void NormalAddressProperty()
        {
            var person = new Person("Big bird", new Address("Seasame Street"));
            Assert.That(person.Address.StreetAddress, Is.Not.Null);
        }

        [Test]
        public void NullObjectAddressProperty()
        {
            var person = new Person("Bill Gates");
            Assert.That(person.Address.StreetAddress, Is.Not.Null);
        }
    }

    public class Person
    {
        public Person(string name)
        {
            Name = name;
            Address = new UnknownAddress();
        }

        public Person(string name, IAddress address)
        {
            Name = name;
            Address = address;
        }

        public string Name { get; private set; }
        public IAddress Address { get; private set; }
    }

    public interface IAddress
    {
        string StreetAddress { get; }
    }

    public class Address : IAddress
    {
        public Address(string streetAddress)
        {
            StreetAddress = streetAddress;
        }

        public string StreetAddress { get; private set; }
    }

    public class UnknownAddress : IAddress
    {
        public string StreetAddress { get { return ""; } }
    }
}
