using Discord;
using UnityEngine;

[Icon("Assets/DiscordBlack.png")]
public class Discord_Controller : MonoBehaviour
{
    [Header("Application")]
    public long applicationID;
    [Space]
    [Header("Details")]
    public string details = "Solo Wandering";
    public string state = "";
    [Space]
    [Header("Image")]
    public string largeImage = "";
    public string largeText = "";

    private long time;

    private static bool instanceExists;
    public Discord.Discord discord;

    void Awake()
    {
        if (!instanceExists)
        {
            instanceExists = true;
            DontDestroyOnLoad(gameObject);
        }
        else if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        discord = new Discord.Discord(applicationID, (System.UInt64)Discord.CreateFlags.NoRequireDiscord);

        time = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();

        UpdateStatus();
    }

    void Update()
    {
        try
        {
            discord.RunCallbacks();
        }
        catch
        {
            Destroy(gameObject);
        }
    }

    void LateUpdate()
    {
        UpdateStatus();
    }

    void UpdateStatus()
    {
        try
        {
            var activityManager = discord.GetActivityManager();
            var activity = new Discord.Activity
            {
                Details = details,
                State = state,
                Assets =
                {
                    LargeImage = largeImage,
                    LargeText = largeText
                },
                Timestamps =
                {
                    Start = time
                }
            };

            activityManager.UpdateActivity(activity, (res) =>
            {
                if (res != Discord.Result.Ok) Debug.LogWarning("Failed connecting to Discord!");
            });
        }
        catch
        {
            Destroy(gameObject);
        }
    }

    void OnApplicationQuit()
    {
        try
        {
            discord.Dispose();
        }
        catch
        {
            Destroy(gameObject);
        }
    }
}