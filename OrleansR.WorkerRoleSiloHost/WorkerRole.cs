using Microsoft.WindowsAzure.ServiceRuntime;
using Orleans.Host;

namespace OrleansR.WorkerRoleSiloHost
{
    public class WorkerRole : RoleEntryPoint
    {
        OrleansAzureSilo silo;

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
