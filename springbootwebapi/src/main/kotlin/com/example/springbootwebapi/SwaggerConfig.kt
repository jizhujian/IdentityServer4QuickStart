package com.example.springbootwebapi

import io.swagger.v3.oas.models.Components
import io.swagger.v3.oas.models.OpenAPI
import io.swagger.v3.oas.models.info.Contact
import io.swagger.v3.oas.models.info.Info
import io.swagger.v3.oas.models.info.License
import io.swagger.v3.oas.models.security.*
import org.springframework.beans.factory.annotation.Value
import org.springframework.context.annotation.Bean
import org.springframework.context.annotation.Configuration

@Configuration
class SwaggerConfig {

    @Value("\${spring.security.oauth2.resourceserver.jwt.issuer-uri}")
    private val authority: String = ""

    @Value("\${springdoc.swagger-ui.oauth.app-name}")
    private val scope: String = ""

    @Bean
    fun  oAuth2Login(): OpenAPI {
        return OpenAPI()
            .addSecurityItem(
                SecurityRequirement()
                    .addList("Bearer Authentication")
            )
            .components(
                Components()
                    .addSecuritySchemes("Bearer Authentication",
                        SecurityScheme()
                            .type(SecurityScheme.Type.OAUTH2)
                            .flows(OAuthFlows()
                                .authorizationCode(OAuthFlow()
                                    .tokenUrl(authority + "/connect/token")
                                    .authorizationUrl(authority + "/connect/authorize")
                                    .scopes(Scopes()
                                        .addString(scope, scope)
                                    )
                                )
                            )
//                            .type(SecurityScheme.Type.HTTP)
//                            .bearerFormat("JWT")
//                            .scheme("bearer")
                    )
            )
            .info(
                Info()
                    .title("身份验证和授权服务快速入门 springboot api")
                    .description("spring boot web + oauth2 + swagger ui")
                    .version("1.0")
                    .termsOfService("服务条款")
                    .contact(Contact().name("张三").email( "zs@hello.world").url("http://localhost"))
                    .license(License().name("许可协议").url("http://localhost"))
            )
    }
}