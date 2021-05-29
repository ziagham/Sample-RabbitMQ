using System;

namespace Contracts
{
    public class CustomerRegistered
    {
        public Guid Id { get; set; }
        public DateTime RegisterUtc { get; set; }
        public string Name { get; set; }
    }
}
