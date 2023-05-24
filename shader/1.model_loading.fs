#version 330 core
out vec4 FragColor;

uniform float shininess;
uniform sampler2D texture_diffuse1;
uniform sampler2D texture_specular1;

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
  
uniform vec3 viewPos;
uniform Light light;
uniform bool boollight;

void main()
{
    if(boollight){
        // ambient
        vec3 ambient = light.ambient * texture(texture_diffuse1, TexCoords).rgb;
    
        // diffuse 
        vec3 norm = normalize(Normal);
        //vec3 lightDir = normalize(light.position - FragPos);
        vec3 lightDir = normalize(-light.direction);
        float diff = max(dot(norm, lightDir), 0.0);
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
    else{
        FragColor = texture(texture_diffuse1, TexCoords);
    }
} 