# 为SqlServer数据库批量添加表注释和列注释
## 1. 配置`yml`类型的配置文件, 格式如下：
```
table: 
  - name: SysUser
    note: 系统用户
    column:
      - name: UserID
        note: 用户表自增ID
      - name: UserCode
        note: 用户代码
      - name: UserName
        note: 用户名称
      - name: UserPwd
        note: 用户密码
      - name: UserMail
        note: 用户邮件
      - name: UserCreate
        note: 条目生成时间
      - name: UserModified
        note: 条目修改时间

  - name: SysMenu
    note: 系统菜单
    column: 
      - name: MenuID
        note: 系统菜单自增ID
      - name: MenuName
        note: 菜单名称
      - name: MenuDesc
        note: 菜单描述
```
## 2. 打开 `insert-sqlserver-notes.exe`
将配置的yml文件导出到系统中
## 3. 系统解析配置文件，生成sql语句。效果如下：
![](http://oqdzx28cd.bkt.clouddn.com/18-1-18/1440409.jpg)