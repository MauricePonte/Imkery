﻿using Imkery.API.Client.Core;
using Imkery.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imkery.API.Client
{
    public class TagDefinitionsClient : BaseCRUDClient<TagDefinition>
    {
        public TagDefinitionsClient(IHttpClientFactory httpClientFactory, IApiConfiguration settings) : base("tagdefinitions", (options) => { }, httpClientFactory, settings)
        {
        }
    }
}
