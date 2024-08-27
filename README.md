# Modular CLI Template

## Instantiate with `dotnet new ...`

The template can easily be used as a template with the `dotnet new` command. To do that, you first need to install it.

1. Clone the repository
2. In the terminal, naviate to the `CliTemplate` directory
3. Run `dotnet new install .` - add the `--force` flag to _update_ the installed template with a newer version of the source

After that, you can instantiate a new project like this.

```shell
dotnet new modwcli -o MyNewCliProject
cd MyNewCliProject
git init
git add .
git commit -m "Initial commit with code from "https://github.com/lord-executor/ModularCliTemplate"
dotnet run
```
