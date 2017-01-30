apiKey = input('Enter a valid Weather Underground API Key: ')

apiKeyFilePaths = ['./Weather.Tests/Resources/WundergroundApiKey.txt']

for path in apiKeyFilePaths:
    with open(path, "w") as apiKeyFile:
        print(apiKey, file=apiKeyFile)

print('Set Weather Underground API Key to', apiKey)

print('\n')

print('Configure complete')
junk = input('Press enter to continue...')
