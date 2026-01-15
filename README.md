# PoC-kafka-Docker-Net
	Modelo exploratorio


<img width="1920" height="1080" alt="KAFKA (2)" src="https://github.com/user-attachments/assets/ad0ab1b6-0d5d-4010-966f-95db6074c996" />

## KafkaConsumerApi – .NET + Kafka + Docker
Este proyecto implementa una API .NET que actúa únicamente como CONSUMER de Kafka. La API no expone endpoints de negocio, solo se levanta, se conecta a Kafka y escucha mensajes de un tópico configurado.

La solución está preparada para funcionar:
✅ Desde Visual Studio (Development / fuera de Docker)
✅ Dentro de Docker (Production)


##LEVANTAR TODOS LOS PROCESOS Y LA API

## 0 CONFIGURACIONES:
	## INSTALAR DOCKER
		-INSTALAR DOCKER DESKTOP
			CMD EJECUTAR: winget install -e --id Docker.DockerDesktop
		-INSTALAR DOCER COMPOSE
		-EN LA CONSOLA CDM VERIFICAR:
		docker --version
		docker compose version

	## INSTALAR KAFKA Y ZOOKEEPER
		-BAJAR IMAGENES EN DOCKER DE KAFKA Y ZOOKEEPER
		docker pull bitnami/kafka:3.6.1
		docker pull bitnami/zookeeper:latest

		EN MI CONSOLA TENGO:
		C:\>docker --version
		Docker version 28.0.4, build b8034c0
		C:\>docker compose version
		Docker Compose version v2.34.0-desktop.1


	## 1 CREAR LA API. O CLONARLA : MI PROYECTO SE LLAMA kafka-consumer-api
		EN EL PROYECTO DEBE ESTAR EL ARCHIVO docker-compose.yml, FUERA DE LA SOLUCION Y DENTRO EL DOCKER FILE (PUBLIC LA API EN DOCKER)

		## CREAR IMAGEN DENTRO DE DOCKER	
		EJECUTAR: docker compose down -v
		EJECUTAR>: docker build -t kafka-consumer-api .

	## LEVANTAR TODO EN DOCKER
    	EJECUTAR: docker compose up -d
		lEVANTA TODO Kafka + Zookeeper + API:
	## VER CONTENEDORES
		docker ps

		DEBE SALIR:
		my-kafka-dotnet-app-dotnet-consumer-api-1
		my-kafka-dotnet-app-kafka-1
		my-kafka-dotnet-app-zookeeper-1


//////////////////////////////////////////////////////////////////////////

## Ejecucion

Opcion A > Levantar Consola Producer -> levantar Kafka -> Levanatra consola Consumer y Levantar proyecto Visual Studio Puerto 8082 (TODO ESTO CORRE DENTRO DE DOCKER)

Opcion B > Levantar Consola Producer -> levantar Kafka -> Levanatra consola Consumer y parar proyecto de .Net desde DOCKER Y Levantar proyecto DESDE  Visual Studio : Puerto 5000 

	## Comandos consola cmd OPCION A

		ENTRAR AL CONTENEDOR:
			docker exec -it my-kafka-dotnet-app-kafka-1 sh
			
		## Enviar mensake Productor
			docker exec -it kafka kafka-console-producer.sh \
			--bootstrap-server localhost:9092 \
			--topic mi-topic

		## Levnatar consumidor de consola
			docker exec -it kafka kafka-console-consumer.sh \
			--bootstrap-server localhost:9092 \
			--topic mi-topic \
			--from-beginning

		## LEVANTAR LA API TAMBIEN DESDE EL CONTENEDOR DE DOCKER

		LUEGO VER EN :
			http://localhost:8082/swagger


	////////////////////////////////////////////
	## Comandos consola cmd OPCION B

		## Levantar docker
			BAJAR TODO: docker compose down -v
			LEVANTAR TODO: docker compose up -d
			PARAR LA API DESDE DOCKER
		
		## Enviar mensake Productor
			docker exec -it kafka kafka-console-producer.sh \
			--bootstrap-server localhost:9092 \
			--topic mi-topic
	
	
		## Levnatar consumidor de consola
			docker exec -it kafka kafka-console-consumer.sh \
			--bootstrap-server localhost:9092 \
			--topic mi-topic \
			--from-beginning
	
		## INICIAR APLICACION DESDE LA API, EN EL ENVIROMENT DEVELOPMENT Y RECORDAD PARA LA APLICACION DESPLEGHADA  EN EL CONTENEDRO DE DOCKER

			LUEGO VER EN :
			http://localhost:5000/swagger
	
