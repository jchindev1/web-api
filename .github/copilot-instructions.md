
# Apply these instructions to all files in the workspace
---
applyTo: "**" 
description: "Guidelines for C#/.NET development with GitHub Copilot."
---

# C#/.NET Project Guidelines for GitHub Copilot

This document provides custom instructions for GitHub Copilot to ensure generated code adheres to our project's .NET best practices, coding standards, and architectural patterns.

## General Principles

* **Prioritize Clarity and Readability:**
    * Use meaningful and descriptive names for variables, methods, classes, and namespaces (e.g., `GetUserByIdAsync`, `OrderService`, `ProductDto`).
    * Keep methods concise and focused on a single responsibility (Single Responsibility Principle).
    * Follow established C# coding conventions (e.g., PascalCase for types and methods, camelCase for local variables).
    * Prefer expression-bodied members for simple properties and methods.

* **Embrace Asynchronous Programming:**
    * For I/O-bound operations (database access, network calls, file I/O), always use `async`/`await` for non-blocking operations.
    * Ensure methods returning `Task` or `Task<T>` are suffixed with `Async`.
    * Avoid `Task.Wait()`, `Task.Result`, or `.GetAwaiter().GetResult()` in synchronous code unless absolutely necessary (e.g., in `Main` for console apps).

* **Dependency Injection (DI):**
    * Favor constructor injection for dependencies.
    * Avoid using service locators or static access to services.
    * Strive for loosely coupled components.

* **Error Handling:**
    * Use structured exception handling (`try-catch-finally`) for anticipated errors.
    * Catch specific exceptions where possible, rather than broad `Exception` catches.
    * Log exceptions with sufficient context.
    * For validation errors, consider returning `ValidationResult` or specific error objects instead of throwing exceptions for control flow.

* **Testing:**
    * When generating code, consider how it will be unit tested.
    * For new features or bug fixes, suggest associated unit tests using `xUnit` and `Moq`.
    * Aim for high test coverage for critical business logic.

* **Performance and Resource Management:**
    * Be mindful of object allocations, especially in high-performance paths.
    * Use `using` statements for `IDisposable` resources (e.g., database connections, file streams).
    * Prefer `IReadOnlyList<T>`, `IReadOnlyDictionary<TKey, TValue>` for collections that should not be modified.

## .NET Specific Guidelines

* **Minimal APIs (for ASP.NET Core 6+):**
    * When generating API endpoints, prefer Minimal APIs where applicable for simpler, more focused endpoints.
    * Use endpoint groups and route handlers for organization.

* **Entity Framework Core:**
    * When suggesting database interactions, use EF Core's asynchronous methods (e.g., `ToListAsync()`, `FirstOrDefaultAsync()`).
    * Consider separation of concerns (e.g., repository pattern or direct `DbContext` usage in services).
    * For queries, prioritize `IQueryable` for deferred execution.

* **Logging:**
    * Use `Microsoft.Extensions.Logging` for structured logging.
    * Inject `ILogger<T>` into classes.
    * Use appropriate log levels (Debug, Information, Warning, Error, Critical).

* **Security:**
    * Sanitize and validate all user inputs to prevent injection attacks (SQL injection, XSS).
    * Use built-in ASP.NET Core authentication and authorization mechanisms.
    * Avoid hardcoding sensitive information.

## CLI Operations and Scripting

* **Favor PowerShell:** When generating command-line interface (CLI) operations, scripts, or examples, **always prefer PowerShell commands over Bash or other shell commands.** This includes common operations like:
    * File system navigation (`Get-ChildItem`, `Set-Location`)
    * Process management (`Get-Process`, `Stop-Process`)
    * Network operations (`Invoke-RestMethod`, `Test-Connection`)
    * Package management (`dotnet tool install`, `nuget install`)
    * Git operations (if not using a GUI client, e.g., `git status` which is often the same, but for specific scripting tasks, prefer PowerShell equivalents if they exist or PowerShell syntax for calling Git).
* **Provide Full Cmdlets:** Instead of aliases, try to provide full cmdlet names for clarity (e.g., `Get-ChildItem` instead of `gci`).
* **Use `-Force` and `-Recurse` where appropriate:** For file operations, consider these parameters for common scenarios.
* **Pipeline Usage:** Demonstrate effective use of the PowerShell pipeline (`|`) for chaining commands.
* **Variable Syntax:** Use `$variableName` for variables.
* **Example:**
    * **Bad (Bash):** `ls -l`
    * **Good (PowerShell):** `Get-ChildItem -Recurse`
    * **Bad (Bash):** `grep "error" log.txt`
    * **Good (PowerShell):** `Select-String -Path log.txt -Pattern "error"`
    * **Bad (Bash):** `rm -rf bin`
    * **Good (PowerShell):** `Remove-Item -Path "bin" -Recurse -Force`

## How to Interact with Copilot

* **Be Specific:** Provide clear and concise prompts.
* **Iterate and Refine:** If the first suggestion isn't perfect, rephrase your prompt or ask for modifications.
* **Review Code:** Always review Copilot's suggestions carefully for correctness, security, and adherence to these guidelines before accepting.

## Example Scenarios/Prompts

* "Generate a C# class for a `Product` entity with properties: `Id` (int), `Name` (string), `Price` (decimal)."
* "Create an `IProductRepository` interface and a `ProductRepository` implementation using EF Core to `GetAllAsync` and `GetByIdAsync`."
* "Write an xUnit test for the `AddProduct` method in `ProductService`."
* "Refactor this synchronous method to use `async`/`await`."
* "Add logging to this method using `ILogger`."
* "Explain the purpose of this C# code snippet."
* "How can I improve the performance of this LINQ query?"
