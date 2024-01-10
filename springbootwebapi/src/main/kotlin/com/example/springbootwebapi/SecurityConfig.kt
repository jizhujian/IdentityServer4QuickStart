package com.example.springbootwebapi

import org.springframework.context.annotation.Bean
import org.springframework.context.annotation.Configuration
import org.springframework.security.config.Customizer
import org.springframework.security.config.annotation.web.builders.HttpSecurity
import org.springframework.security.config.annotation.web.configuration.EnableWebSecurity
import org.springframework.security.oauth2.core.authorization.OAuth2AuthorizationManagers.hasScope
import org.springframework.security.web.SecurityFilterChain

@Configuration
@EnableWebSecurity
public class SecurityConfig {
    @Bean
    fun oAuth2Authentication(http: HttpSecurity): SecurityFilterChain {
        http
            .authorizeHttpRequests { authorize -> authorize
                .requestMatchers("/api/**").access(hasScope("身份验证和授权服务快速入门_api"))
                .anyRequest().anonymous()
            }
            .oauth2ResourceServer { oauth2 -> oauth2
                .jwt(Customizer.withDefaults())
            }
        return http.build()
    }
}
