using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

public static class TaskExtensionMethods
{
    public static async void WrapErrors(this UniTask task)
    {
        await task;
    }

    public static async void WrapErrors(this Task task)
    {
        await task;
    }
}