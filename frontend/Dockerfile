# Build Stage
FROM node:18-alpine AS build
WORKDIR /app

# Copiar package files
COPY package*.json ./

# Instalar dependências (inclui devDependencies para o build)
RUN npm install

# Copiar código fonte e fazer build
COPY . .

# Permitir injetar URL da API no build, se necessário
ARG REACT_APP_API_URL
ENV REACT_APP_API_URL=$REACT_APP_API_URL

RUN npm run build

# Production Stage com Nginx
FROM nginx:alpine AS production
WORKDIR /usr/share/nginx/html

# Remover arquivos default do nginx
RUN rm -rf ./*

# Copiar build do React
COPY --from=build /app/build .

# Copiar configuração customizada do nginx
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Expor porta 80
EXPOSE 80

# Start nginx
CMD ["nginx", "-g", "daemon off;"]

