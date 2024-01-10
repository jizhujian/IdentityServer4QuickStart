FROM eclipse-temurin:21.0.1_12-jre
VOLUME /tmp
ARG JAVA_OPTS
ENV JAVA_OPTS=$JAVA_OPTS
COPY target/springbootwebapi-0.0.1-SNAPSHOT.jar springbootwebapi.jar
EXPOSE 39995
ENTRYPOINT exec java $JAVA_OPTS -jar springbootwebapi.jar
# For Spring-Boot project, use the entrypoint below to reduce Tomcat startup time.
#ENTRYPOINT exec java $JAVA_OPTS -Djava.security.egd=file:/dev/./urandom -jar springbootwebapi.jar
