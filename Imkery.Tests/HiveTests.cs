using Imkery.Data.Storage;
using Imkery.Data.Storage.Core;
using Imkery.Entities;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imkery.Tests
{
    public class HiveTests
    {
        [Test]
        public async Task SaveHive()
        {
            Hive hive = new Hive();
            (BootstrapTests.ServiceProvider.GetService<IImkeryUserProvider>() as MockUserProvider).LoggedIn = true;
            hive.Identifier = "Tester";
            var repository = BootstrapTests.ServiceProvider.GetService<HivesRepository>();
            var savedHive = await repository.AddAsync(hive);
            Assert.That(savedHive.Id != Guid.Empty, "hive not saved");
            Assert.That(savedHive.OwnerId == new Guid(MockUserProvider.DefaultUser.Id), "User not automaticaly set");
        }


        [Test]
        public async Task DeleteHive()
        {
            var dbContext = BootstrapTests.ServiceProvider.GetService<ImkeryDbContext>();
            dbContext.Hives.RemoveRange(dbContext.Hives);
            dbContext.SaveChanges();

            Hive hive = new Hive();
            (BootstrapTests.ServiceProvider.GetService<IImkeryUserProvider>() as MockUserProvider).LoggedIn = true;
            hive.Identifier = "Tester";
            var repository = BootstrapTests.ServiceProvider.GetService<HivesRepository>();
            var savedHive = await repository.AddAsync(hive);
            await repository.DeleteAsync(hive);
            Assert.That((await repository.GetCollectionAsync(0, int.MaxValue, "", true, new Dictionary<string, string>(), new string[0])).Count == 0, "Not deleted");
        }

        [Test]
        public async Task UpdateHive()
        {
            Hive hive = new Hive();
            (BootstrapTests.ServiceProvider.GetService<IImkeryUserProvider>() as MockUserProvider).LoggedIn = true;
            hive.Identifier = "Tester";
            var repository = BootstrapTests.ServiceProvider.GetService<HivesRepository>();
            var savedHive = await repository.AddAsync(hive);
            savedHive = await repository.GetItemByIdAsync(hive.Id);

            savedHive.Identifier = "Tester2";
            await repository.UpdateAsync(savedHive.Id, savedHive);
            savedHive = await repository.GetItemByIdAsync(savedHive.Id);
            Assert.That(savedHive.Identifier == "Tester2", "Update not working");
        }



        [Test]
        public async Task ApplyActionToHive()
        {
            Hive hive = new Hive();
            (BootstrapTests.ServiceProvider.GetService<IImkeryUserProvider>() as MockUserProvider).LoggedIn = true;
            hive.Identifier = "Tester";
            var repository = BootstrapTests.ServiceProvider.GetService<HivesRepository>();
            var savedHive = await repository.AddAsync(hive);

            TagDefinition tagDefinition = new TagDefinition();
            tagDefinition.Name = "Test";
            var tagRepository = BootstrapTests.ServiceProvider.GetService<TagDefinitionsRepository>();
            tagDefinition = await tagRepository.AddAsync(tagDefinition);

            TagDefinition tagDefinitionWithLimit = new TagDefinition();
            tagDefinitionWithLimit.Name = "Test2";
            tagDefinitionWithLimit = await tagRepository.AddAsync(tagDefinitionWithLimit);

            ActionDefinition actionDefinition = new ActionDefinition();
            actionDefinition.Name = "Test";
            actionDefinition.TagLinks = new List<TagLink>()
            {
                new TagLink()
                {
                    IsContinues =true,
                    TagDefinitionId = tagDefinition.Id
                },
                  new TagLink()
                {
                    IsContinues =false,
                    Duration ="01:00",
                    TagDefinitionId = tagDefinitionWithLimit.Id
                }
            };
            var actionRepository = BootstrapTests.ServiceProvider.GetService<ActionDefinitionsRepository>();
            actionDefinition = await actionRepository.AddAsync(actionDefinition);
            actionDefinition = await actionRepository.GetItemByIdAsync(actionDefinition.Id);


            savedHive = await repository.GetItemByIdAsync(hive.Id);
            var appliedActionToHive = await repository.ApplyActionToHiveAsync(savedHive, actionDefinition);
            Assert.That(appliedActionToHive.Tags.Where(b => b.AlwaysValid).Count() == 1, "Tag not created right");
            Assert.That(appliedActionToHive.Tags.Where(b => (b.ValidTill - DateTime.UtcNow).Minutes > 30).Count() == 1, "Tag not created right");
        }
    }
}
