namespace Hotel.Common.Synchronization;

public static class SynchronizationExamples
{
    private static readonly object LockObject = new();
    private static int _sharedCounter;

    public static void LockExample()
    {
        _sharedCounter = 0;

        Parallel.For(0, 10_000, _ =>
        {
            lock (LockObject)
            {
                _sharedCounter++; // без lock тут можлива "гонка даних" (race condition)
            }
        });

        Console.WriteLine($"[lock] Очікували 10000, отримали: {_sharedCounter}");
    }

    public static async Task SemaphoreExample()
    {
        // дозволяємо не більше 3 "одночасних візитів" до ресурсу
        using var semaphore = new SemaphoreSlim(initialCount: 3, maxCount: 3);
        var tasks = new List<Task>();

        for (int i = 1; i <= 6; i++)
        {
            int taskId = i;
            tasks.Add(Task.Run(async () =>
            {
                await semaphore.WaitAsync();
                try
                {
                    Console.WriteLine($"[Semaphore] Задача {taskId} увійшла в критичну секцію.");
                    await Task.Delay(200); // імітація роботи
                    Console.WriteLine($"[Semaphore] Задача {taskId} завершила роботу.");
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }

        await Task.WhenAll(tasks);
    }

    public static void AutoResetEventExample()
    {
        using var signal = new AutoResetEvent(initialState: false);

        var consumer = new Thread(() =>
        {
            Console.WriteLine("[AutoResetEvent] Споживач чекає на сигнал...");
            signal.WaitOne(); // блокується, поки виробник не викличе Set()
            Console.WriteLine("[AutoResetEvent] Споживач отримав сигнал і продовжив роботу.");
        });

        consumer.Start();

        Thread.Sleep(300); // імітація "виробництва" даних
        Console.WriteLine("[AutoResetEvent] Виробник подає сигнал.");
        signal.Set();

        consumer.Join();
    }

    public static void MonitorExample()
    {
        bool lockTaken = false;
        try
        {
            Monitor.Enter(LockObject, ref lockTaken);
            Console.WriteLine("[Monitor] Увійшли в критичну секцію через Monitor.Enter.");
        }
        finally
        {
            if (lockTaken) Monitor.Exit(LockObject);
        }
    }
}