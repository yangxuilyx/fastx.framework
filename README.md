## FastX

- [x] 模块化
- [x] 依赖注入
  - [x] Autofac
- [x] 数据访问
  - [x] SqlSugar
    - [x] 仓储
    - [x] 数据过滤
    - [x] 软删除
    - [x] 自动建表
    - [x] Ulid 支持
    - [ ] 种子数据
  - [ ] MongoDB
- [x] 缓存
- [x] 多租户
  - [x] 数据过滤
- [ ] 审计
  - [ ] 实体审计
  - [ ] 审计日志
- [ ] 认证
  - [ ] OpenIddict
  - [x] 当前用户
- [x] 授权
  - [x] 权限系统
- [x] 应用服务
  - [x] 对象映射
  - [x] 自动 Api 控制器
  - [x] Crud 应用服务
- [ ] 实时
  - [ ] Signalr
  - [ ] 分布式实时系统
- [x] 全局异常处理
- [ ] 日志
- [ ] 后台服务
  - [ ] 后台作业
  - [ ] 后台工作者
- [x] UI
  - [x] SwaggerUI

## angular

- [x] 登录
- [ ] 实时消息
- [x] 用户管理
- [x] 角色管理
- [x] 权限管理
- [ ] 代码生成器

## 安装 docker

```
dnf install-y yum-config-manager
yum-config-manager \
    --add-repo \
    http://mirrors.aliyun.com/docker-ce/linux/centos/docker-ce.repo
yum  -y docker-ce docker-ce-cli containerd.io
systemctl enable docker --now

sudo mkdir -p /etc/docker
sudo tee /etc/docker/daemon.json <<-'EOF'
{
  "registry-mirrors": ["https://docker.miniecc.top"]
}
EOF
sudo systemctl daemon-reload
sudo systemctl restart docker

```

## docker 启动命令

--privileged=true

### pgsql

```
docker run -d -p 5432:5432 --name postgres --restart=always -v /etc/localtime:/etc/localtime -v pgdata:/var/lib/postgresql/data -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=PasSWord#0822 postgres:14.13
```

### redis

```
docker run --restart=always -p 6379:6379 --name redis -d redis:7.4.0
```

### nginx

```
docker run -itd --name nginx -p 80:80 -p 443:443 -v /app/nginx:/etc/nginx/conf.d nginx
```

### mysql

```
docker run -d -p 3306:3306 --name mysql --restart=always -v /etc/localtime:/etc/localtime -e MYSQL_ROOT_PASSWORD=123456 -v mysqldata:/var/lib/mysql -v mysqlconf:/etc/mysql/conf.d mysql:8.0 --character-set-server=utf8mb4 --collation-server=utf8mb4_unicode_ci

```

### acme

```

acme.sh --install-cert -d xxh.zcwlkjyxgs.cn \
 --key-file /app/nginx/ssl/xxh_key.pem \
 --fullchain-file /app/nginx/ssl/xxh_cert.pem \
 --reloadcmd "docker restart nginx"

```

### changlsteel

```
docker rm -f changlsteel
docker run -it -d --name=changlsteel -p 88:8080 -e ASPNETCORE_ENVIRONMENT=Production  -v /app/data:/app/wwwroot registry.cn-hangzhou.aliyuncs.com/fastx/fastx:changlsteel_1.0.36


```
