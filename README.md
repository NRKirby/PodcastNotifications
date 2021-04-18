# Podcast Notifications

https://podcast-notifications.vercel.app/

Receive email notifications when podcast episodes get published. 

# Tech

Built with:

- [Azure functions](https://docs.microsoft.com/en-us/azure/azure-functions/)
- [Azure table storage](https://azure.microsoft.com/en-gb/services/storage/tables/)
- [Next.js](https://nextjs.org/)
- [TailwindUI](https://tailwindui.com/)

# Other info

The app depends on an external service `FeedContentStorage` that stores RSS feed content and sends notifications when new content is published. This service is currently closed-source.