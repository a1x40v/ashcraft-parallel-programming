using ProducerConsumerRssFeeds.Models;

FeedAggregator aggregator = new();

var items = await aggregator.GetAllMicrosoftBlogPosts();

foreach (var item in items)
{
    Console.WriteLine(item.PostDate);
}