using Microsoft.WindowsAzure.ServiceRuntime;
using Orleans.Host.Azure;

namespace OrleansR.WorkerRoleSiloHost
{
    public class WorkerRole : RoleEntryPoint
    {
        Orleans.Host.Azure.OrleansAzureSilo silo;

        public override bool OnStart()
        {
            // Do other silo initialization – for example: Azure diagnostics, etc 

            silo = new OrleansAzureSilo();

            return silo.Start(RoleEnvironment.DeploymentId, RoleEnvironment.CurrentRoleInstance);
        }
        public override void OnStop() { silo.Stop(); }
        public override void Run() { silo.Run(); }
    }
}
