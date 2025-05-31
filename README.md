### Notes
Should switches and services determine and apply state be the only things that change room state?

### Update code gen tool
 ```
 dotnet tool update NetDaemon.HassModel.CodeGen
 ```
 
### Execute code gen tool
```
dotnet tool run nd-codegen
```

### LLMs
#### Ollama
Docker
```bash

docker run -d --name ollama \
  -p 11434:11434 \
  -v ollama-data:/root/.ollama \
  ollama/ollama
```

Docker-Compose
```bash

```

List models
```bash

curl http://localhost:11434/api/tags
```

Download phi model (~4.6GB of free ram needed)
```bash

curl http://localhost:11434/api/pull -d '{ "name": "phi" }'
```

Download TinyLlama model (~2GB of free ram needed)
```bash

curl http://localhost:11434/api/pull -d '{ "name": "tinyllama" }'
```

Example where the result is streamed
```bash

curl http://127.0.0.1:11434/api/generate -d '{
  "model": "tinyllama",
  "prompt": "Can you tell me a joke?"
}'
```

Example where the result is NOT streamed
```bash

curl http://127.0.0.1:11434/api/generate -d '{
  "model": "tinyllama",
  "prompt": "Can you tell me a joke?",
  "stream": false
}'
```