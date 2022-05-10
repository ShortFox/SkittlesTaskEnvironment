interface ITask
{
    void Initiate();                // Initiate Task
    void Setup();                   // Set up Task
    void Run();                     // Run Task.
    void CheckState();              // Check state of task
    void Cleanup();                 // Clean up Task
    void Finish();                  // End Task
}
