package com.example.springbootwebapi

import io.swagger.v3.oas.annotations.Operation
import io.swagger.v3.oas.annotations.media.ArraySchema
import io.swagger.v3.oas.annotations.media.Content
import io.swagger.v3.oas.annotations.media.Schema
import io.swagger.v3.oas.annotations.responses.ApiResponse
import io.swagger.v3.oas.annotations.responses.ApiResponses
import io.swagger.v3.oas.annotations.tags.Tag
import org.springframework.http.MediaType
import org.springframework.security.oauth2.server.resource.authentication.JwtAuthenticationToken
import org.springframework.web.bind.annotation.GetMapping
import org.springframework.web.bind.annotation.RequestMapping
import org.springframework.web.bind.annotation.RestController

@RestController
@RequestMapping("/api/identity")
@Tag(name="Identity", description = "用户标识")
class IdentityController {

    @GetMapping("/getuserclaims", produces=["application/json"])
    @Operation(summary = "获取用户声明")
    fun getUserClaims(jwtToken: JwtAuthenticationToken): List<Pair<String,Any>> {
        val info = mutableListOf<Pair<String,Any>>()
        jwtToken.tokenAttributes.forEach {
            if (it.value is Collection<*>) {
                val value = it.value as Collection<Any>
                value.forEach { item ->
                    info.add(it.key to item)
                }
            }
            else {
                    info.add(it.key to it.value)
                }
        }
        return info
    }
}