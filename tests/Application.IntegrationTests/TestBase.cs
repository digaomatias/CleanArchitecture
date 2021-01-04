using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Application.IntegrationTests
{
    using static IntegrationFixture;

    public class TestBase : IAsyncLifetime
    {
        public async Task InitializeAsync()
        {
            await ResetState();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
