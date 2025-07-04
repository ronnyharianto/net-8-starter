prefix=net-starter-api
echo "Cleaning up Docker images for: $prefix"

docker images --format '{{.Repository}}:{{.Tag}} {{.CreatedAt}}' | \
  grep "^$prefix:" | \
  sort -rk2 | \
  awk '{print $1}' | \
  tail -n +6 | \
  xargs -r docker rmi