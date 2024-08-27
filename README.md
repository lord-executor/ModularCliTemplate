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

## Creating Commands with `dotnet new ...`

1. Navigate to the `CommandTemplate` directory
2. Run `dotnent new install . --force` to install / update the template

In your modular CLI project, you can now create a new command with an arguments and handler class with

```shell
dotnet new modclicmd -n Hello -N "CliTemplate.Hello" -C "Say hello to everybody" -o "Hello"
```

This will create a new `HelloCommand` together with a `HelloArgs` and `HelloHandler` class in the "Hello" subdirectory (with the `-o` flag) and namespace `CliTemplate.Hello`. **Don't forget** to register the new command in `Program.cs`.
