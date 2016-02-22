apiKey = input('Enter a valid Weather Underground API Key: ')

apiKeyFilePath = "./Weather/Resources/WeatherUndergroundAPIKey.txt"

with open(apiKeyFilePath, "w") as apiKeyFile:
    print(apiKey, file=apiKeyFile)

print('Set Weather Underground API Key to', apiKey, 'in file', apiKeyFilePath)
print('Remember to never commit this file to source control. It is ignored by the .gitignore normally.')

print('\n')

print('Configure complete')
junk = input('Press enter to continue...')