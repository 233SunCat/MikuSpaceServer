﻿using Microsoft.AspNetCore.Identity;

namespace MikuSpaceServer.Models
{
    public class User:IdentityUser<Guid>
    {
        public DateTime CreationTime { get; init; }

        public DateTime? DeletionTime { get; private set; }

        public bool IsDeleted { get; private set; }

        public User(string userName) : base(userName)
        {
            Id = Guid.NewGuid();
            CreationTime = DateTime.Now;
        }

        public void SoftDelete()
        {
            this.IsDeleted = true;
            this.DeletionTime = DateTime.Now;
        }
    }
}
