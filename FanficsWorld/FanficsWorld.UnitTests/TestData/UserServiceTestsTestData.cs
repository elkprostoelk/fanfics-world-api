using System.Collections;
using FanficsWorld.DataAccess.Entities;

namespace FanficsWorld.UnitTests.TestData;

public class UserServiceTestsTestData : IEnumerable<object?[]>
{
    public IEnumerator<object?[]> GetEnumerator()
    {
        yield return
        [
            null, 5, new List<User>
            {
                new() { Id = "b2d2935a-0500-48f7-9981-8dec46572278", UserName = "John Doe", Email = "user1@mail.com" },
                new() { Id = "25d71939-a363-46fe-8e2e-e5b151a4883f", UserName = "Name2 Surname2", Email = "user2@mail.com" },
                new() { Id = "7d74f5a0-fe9f-4e69-93b8-725f66591708", UserName = "Jack Never", Email = "user3@mail.com" },
                new() { Id = "58787929-28b5-46a7-b7ee-2d21c4a2cb8e", UserName = "Amy Doe", Email = "amydoe@mail.com" },
                new() { Id = "69197b59-ca70-426c-aab5-ed35b4a9ea62", UserName = "Name5 Surname5", Email = "user5@mail.com" }
            }
        ];
        yield return
        [
            "7d74f5a0-fe9f-4e69-93b8-725f66591708", 4, new List<User>
            {
                new() { Id = "b2d2935a-0500-48f7-9981-8dec46572278", UserName = "John Doe", Email = "user1@mail.com" },
                new() { Id = "25d71939-a363-46fe-8e2e-e5b151a4883f", UserName = "Name2 Surname2", Email = "user2@mail.com" },
                new() { Id = "58787929-28b5-46a7-b7ee-2d21c4a2cb8e", UserName = "Amy Doe", Email = "amydoe@mail.com" },
                new() { Id = "69197b59-ca70-426c-aab5-ed35b4a9ea62", UserName = "Name5 Surname5", Email = "user5@mail.com" }
            }
        ];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}