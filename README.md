# DayWriter

A setup for DevOps knowledge assessment.

## Description

The DayWriter application creates a table in a Postgres database and once a minute adds the date in that table.
Build and deploy a Postgres database and the application in a Kubernetes cluster and verify that the application is updating the database correctly.

## Tasks

1. Create a Dockerfile to build an image of this dotnet app.
2. Bitnami is deprecating its free docker registry. Make any necessary changes to the repo to make use of their new free registry.
3. There is an existing helm chart that creates a deployment and a service for the DayWriter application.
    * The application requires a connection string(represented by the connectionString variable in the Program.cs file). Please add that as an environment variable and pass it in the application.
    * Fix any issues in the helm chart, the release can be named daywriter.
    * Make sure that this application can be deployed in both a development and a production environment in a different namespace on the same local cluster where the deployment will be tested. Each running pod of the app should have an environment variable called `ENV` that would represent the environment (development or production)
    * We want to deploy the application on specific nodes in the cluster that have the `env=<environment>` label and the `<environment>=true:NoSchedule` taint. Please add the necessary fields in the helm chart to achieve that
    * We want to have 2 replicas of the application running in dev and 3 in prod, but we don't want to schedule them at the same node, i.e dev pods should be on different nodes between them.
    * We need to make sure that at least 1 replica is always running in dev and 2 in prod, add the necessary configuration to the helm chart to achieve that.
4. Build and deploy the application along with a postgres pod.
    * We want to be able to deploy both those applications at the same time in a development and production environment. You should use a different postgres for each environment.
5. Create Github workflow files to build and deploy those applications in both the development and production environment. (You don't have to execute them)

## Notes

* Solutions to the tasks should be in different commits.
* While testing locally, the last 3 points of Question 3 can be ignored if not able to configure nodes, but you should provide the configuration that would be necessary to achieve that.
* The applications should be running in a Kubernetes cluster.
* The docker-compose yaml file is not expected to run but can be used for testing the containers.
* You can use any tool to build and deploy these applications but in the case that you want to use skaffold, there is a skaffold file provided that contains the definition for the postgres release, you can update it as necessary to include the DayWriter application as well.
* You can use minikube or any other tool of your choice to test those applications locally to verify that they work.
