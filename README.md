# LUIS-Dynamic-Intent-Bot #

## Scenario: ##
When a bot is created with Azure Bot services then its intents are defined in LUIS (http://luis.ai) which are not dynamic. If there is a new intent that our bot needs to understand then a developer has to goto LUIS app and add a new intent (and utterences) and then train the app with new data.

## Solution: ##
To solve this problem I came up with following technique:
1. Create a Language Unterstanding Bot using Azure Bot Services(Bot Framework). 
2. This bot should be connected to LUIS, which will take a sentance and return its intention.
3. if the intention is None, meanign bot is not able to understand the intention then we can goto SharePoint Online list to see if intentions are mentioend there and can write code inside bot to perform appropraite functionality.

## Note: ##
The code is provided in C# due to ease of SharePoint CSOM(Client Side Object Model) authenticaation support.
