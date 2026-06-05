#!/bin/bash

set -euxo pipefail

usage() {
  echo "Uso: ./scafold-service.sh NomeDoServico"
  echo
  echo "Exemplo:"
  echo "  ./scafold-service.sh fire"
}

if [ -z "$1" ]; then
  usage
  exit 1
fi

SERVICE_PATH=$1
SERVICE_NAME="${SERVICE_PATH##*/}"

SRC_BASE="src/$SERVICE_PATH"
DOMAIN="$SRC_BASE/$SERVICE_NAME.Domain"
APPLICATION="$SRC_BASE/$SERVICE_NAME.Application"
INFRA="$SRC_BASE/$SERVICE_NAME.Infrastructure"
API="$SRC_BASE/$SERVICE_NAME.Api"

UNIT_TEST="tests/$SERVICE_NAME/$SERVICE_NAME.UnitTests"
INTEGRATION_TEST="tests/$SERVICE_NAME/$SERVICE_NAME.IntegrationTests"

echo "Criando microsserviço $SERVICE_NAME..."


strip_versions() {
  local file="$1"

  sed -i 's/ Version="[^"]*"//g' "$file"
}

create_solution_folder() {
  mkdir -p "$SRC_BASE"
}

create_project_folders() {
  echo "Criando estrutura de diretórios..."

  mkdir -p \
    "$INFRA/Repositories" \
    "$INFRA/EntityConfigurations" \
    "$DOMAIN/Interfaces" \
    "$DOMAIN/Entities" \
    "$APPLICATION/DTOs" \
    "$API/Middleware"
}

create_service_projects() {
  dotnet new classlib -n "$SERVICE_NAME.Domain" -o "$DOMAIN" --no-restore
  dotnet new classlib -n "$SERVICE_NAME.Application" -o "$APPLICATION" --no-restore
  dotnet new classlib -n "$SERVICE_NAME.Infrastructure" -o "$INFRA" --no-restore
  dotnet new webapi -n "$SERVICE_NAME.Api" -o "$API" --no-restore

  strip_versions "$API/$SERVICE_NAME.Api.csproj"
}

add_project_references() {
  dotnet add "$APPLICATION/$SERVICE_NAME.Application.csproj" reference \
    "$DOMAIN/$SERVICE_NAME.Domain.csproj"

  dotnet add "$INFRA/$SERVICE_NAME.Infrastructure.csproj" reference \
    "$APPLICATION/$SERVICE_NAME.Application.csproj"

  dotnet add "$API/$SERVICE_NAME.Api.csproj" reference \
    "$APPLICATION/$SERVICE_NAME.Application.csproj"

  dotnet add "$API/$SERVICE_NAME.Api.csproj" reference \
    "$INFRA/$SERVICE_NAME.Infrastructure.csproj"
}

create_test_projects() {
  echo "Criando projetos de teste..."

  dotnet new xunit -n "$SERVICE_NAME.UnitTests" -o "$UNIT_TEST" --no-restore
  dotnet new xunit -n "$SERVICE_NAME.IntegrationTests" -o "$INTEGRATION_TEST" --no-restore

  strip_versions "$UNIT_TEST/$SERVICE_NAME.UnitTests.csproj"
  strip_versions "$INTEGRATION_TEST/$SERVICE_NAME.IntegrationTests.csproj"
}

add_test_references() {
  dotnet add "$UNIT_TEST/$SERVICE_NAME.UnitTests.csproj" reference \
    "$DOMAIN/$SERVICE_NAME.Domain.csproj" \
    "$APPLICATION/$SERVICE_NAME.Application.csproj"

  dotnet add "$INTEGRATION_TEST/$SERVICE_NAME.IntegrationTests.csproj" reference \
    "$APPLICATION/$SERVICE_NAME.Application.csproj" \
    "$INFRA/$SERVICE_NAME.Infrastructure.csproj"
}

add_test_packages() {
  dotnet add "$UNIT_TEST/$SERVICE_NAME.UnitTests.csproj" package Moq --no-restore
  dotnet add "$UNIT_TEST/$SERVICE_NAME.UnitTests.csproj" package FluentAssertions --no-restore

  strip_versions "$UNIT_TEST/$SERVICE_NAME.UnitTests.csproj"
}

add_projects_to_solution() {
  echo "Adicionando projetos na solution..."

  dotnet sln add "$DOMAIN/$SERVICE_NAME.Domain.csproj"
  dotnet sln add "$APPLICATION/$SERVICE_NAME.Application.csproj"
  dotnet sln add "$INFRA/$SERVICE_NAME.Infrastructure.csproj"
  dotnet sln add "$API/$SERVICE_NAME.Api.csproj"

  dotnet sln add "$UNIT_TEST/$SERVICE_NAME.UnitTests.csproj"
  dotnet sln add "$INTEGRATION_TEST/$SERVICE_NAME.IntegrationTests.csproj"
}

restore_packages() {
  echo "Restaurando pacotes..."

  dotnet restore
}


create_solution_folder
create_service_projects
create_project_folders
add_project_references
create_test_projects
add_test_references
add_test_packages
add_projects_to_solution
restore_packages

echo "Microserviço $SERVICE_NAME criado com sucesso."