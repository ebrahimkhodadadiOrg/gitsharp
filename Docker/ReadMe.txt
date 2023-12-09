1- install Docker & Docker-Compose
2- copy docker-compose.yml file (this file in this folder have Mongo database & redis Database & ElasticSearch and logstash and kibana)
3- run with this commands
$ sudo sysctl -w vm.max_map_count=262144
$ sudo docker-compose up -d
4-get list of containers
$ sudo docker ps -a
5- if want to stop all containers
$ sudo docker-compose down -d
    - name: remove old images
      run: docker system prune -a -f