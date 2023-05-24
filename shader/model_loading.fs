#version 330 core
out vec4 FragColor;

uniform float shininess;
uniform sampler2D texture_diffuse1;
uniform sampler2D texture_specular1;
uniform sampler2D normalMap;

struct Light {
    vec3 position;  
    vec3 direction;
  
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    float constant;
    float linear;
    float quadratic;
};

in vec3 FragPos;  
in vec3 Normal;  
in vec2 TexCoords;
in vec3 Tangent;
in vec3 Bitangent;
in vec3 TangentLightPos;
in vec3 TangentViewPos;
in vec3 TangentFragPos;

uniform vec3 viewPos;
uniform Light light;
uniform bool boollight;
uniform bool nm;

void main()
{
    if(boollight){
        if(nm){
            vec3 normal = texture(normalMap, TexCoords).rgb;
            normal = normalize(normal * 2.0 - 1.0);

            // get diffuse color
            vec3 color = texture(texture_diffuse1, TexCoords).rgb;

            // ambient
            vec3 ambient = 0.1 * color;
    
            // diffuse 
            vec3 lightDir = normalize(TangentLightPos - TangentFragPos);
            float diff = max(dot(lightDir, normal), 0.0);
            vec3 diffuse = diff * color;  
    
            // specular
            vec3 viewDir = normalize(TangentViewPos - TangentFragPos);
            vec3 reflectDir = reflect(-lightDir, normal);  
            vec3 halfwayDir = normalize(lightDir + viewDir);
            float spec = pow(max(dot(viewDir, reflectDir), 0.0), shininess);
            vec3 specular = vec3(0.2) * spec;
     
            vec3 result = ambient + diffuse + specular;
            FragColor = vec4(result, 1.0);
        }
        else {
            vec3 normal = texture(normalMap, TexCoords).rgb;
            normal = normalize(normal * 2.0 - 1.0);

            // ambient
            vec3 ambient = light.ambient * texture(texture_diffuse1, TexCoords).rgb;
    
            // diffuse 
            vec3 norm = normalize(Normal);
            //vec3 lightDir = normalize(light.position - FragPos);
            vec3 lightDir = normalize(-light.direction);
            //float diff = max(dot(lightDir, norm), 0.0);
            float diff = max(dot(lightDir, norm), 0.0);
            vec3 diffuse = light.diffuse * diff * texture(texture_diffuse1, TexCoords).rgb;  
    
            // specular
            vec3 viewDir = normalize(- FragPos);
            vec3 reflectDir = reflect(-lightDir, norm);  
            float spec = pow(max(dot(viewDir, reflectDir), 0.0), shininess);
            vec3 specular = light.specular * spec * texture(texture_specular1, TexCoords).rgb;

            // attenuation
            //float distance    = length(light.position - FragPos);
            //float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));    
            //ambient  *= attenuation; 
            //diffuse   *= attenuation;
            //specular *= attenuation;
     
            vec3 result = ambient + diffuse + specular;
            FragColor = vec4(result, 1.0);
        }
    }      
    else{
        FragColor = texture(texture_diffuse1, TexCoords);
    }
} 