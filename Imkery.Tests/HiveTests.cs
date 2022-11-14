using Imkery.Data.Storage;
using Imkery.Data.Storage.Core;
using Imkery.Entities;
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
            Assert.That(appliedActionToHive.Tags.First().AlwaysValid, "Tag not created right");
            Assert.That((appliedActionToHive.Tags.Last().ValidTill - DateTime.UtcNow).Minutes > 30, "Tag not created right");
        }
    }
}
