# 🚀 HƯỚNG DẪN PUSH CODE LÊN GITHUB

## ✅ BẠN ĐÃ CÓ:
- GitHub account ✅
- Repository đã tạo ✅
- Code trong folder `D:\TrainingB` ✅

---

## 📋 BƯỚC 4: PUSH CODE LÊN GITHUB

### **1. Mở PowerShell trong folder TrainingB**

**Cách 1:**
- Mở folder `D:\TrainingB` trong File Explorer
- Shift + Right-click vào vùng trống
- Chọn "Open PowerShell window here"

**Cách 2:**
- Mở PowerShell
- Gõ:
```powershell
cd D:\TrainingB
```

---

### **2. Kiểm tra Git đã cài chưa**

```powershell
git --version
```

**Nếu thấy:** `git version 2.x.x` → OK ✅  
**Nếu lỗi:** Cần cài Git (xem bước 2.1)

---

### **2.1. Cài Git (nếu chưa có)**

1. Download: https://git-scm.com/download/win
2. Chạy installer
3. Chọn tất cả options mặc định
4. Restart PowerShell
5. Test lại: `git --version`

---

### **3. Initialize Git repository**

```powershell
git init
```

**Kết quả:** `Initialized empty Git repository in D:/TrainingB/.git/`

---

### **4. Config Git user (lần đầu tiên)**

```powershell
git config --global user.name "Your Name"
git config --global user.email "your-email@example.com"
```

**Thay:**
- `Your Name` → Tên bạn (vd: "Nguyen Van A")
- `your-email@example.com` → Email GitHub của bạn

---

### **5. Add tất cả files**

```powershell
git add .
```

**Giải thích:** Dấu `.` = tất cả files trong folder

---

### **6. Commit (lưu snapshot)**

```powershell
git commit -m "Initial commit - TrainingB Web App"
```

**Kết quả:** Sẽ show số files đã commit

---

### **7. Add remote repository**

```powershell
git remote add origin https://github.com/YOUR_USERNAME/TrainingB.git
```

**⚠️ QUAN TRỌNG:**
- Thay `YOUR_USERNAME` bằng username GitHub của bạn!
- Ví dụ: `https://github.com/nguyenvana/TrainingB.git`

**Kiểm tra URL:**
- Vào repository trên GitHub
- Click nút "Code" màu xanh
- Copy URL từ đó!

---

### **8. Rename branch thành main**

```powershell
git branch -M main
```

---

### **9. Push code lên GitHub**

```powershell
git push -u origin main
```

**Sẽ hỏi login:**
- Username: GitHub username
- Password: **Personal Access Token** (KHÔNG phải password!)

---

### **9.1. Tạo Personal Access Token**

**Nếu GitHub hỏi password:**

1. Vào: https://github.com/settings/tokens
2. Click: **"Generate new token"** → **"Generate new token (classic)"**
3. Note: "TrainingB deployment"
4. Expiration: **No expiration** hoặc 90 days
5. Scopes: Tick **"repo"** (tất cả repo options)
6. Click: **"Generate token"**
7. **COPY TOKEN NGAY!** (chỉ hiện 1 lần)
8. Dùng token này làm password khi push

---

### **10. Verify trên GitHub**

1. Mở browser → GitHub repository
2. Refresh page
3. Sẽ thấy tất cả files đã upload! ✅

**Files cần có:**
- `Dockerfile`
- `.dockerignore`
- `.gitignore`
- `README.md`
- `TrainingB.Core/`
- `TrainingB.Web/`
- `appsettings.json`
- v.v...

---

## ✅ XONG BƯỚC 4!

**Tiếp theo:**
- Bước 5: Deploy lên Render.com
- Xem file: `DEPLOY_TO_RENDER.md`

---

## 🚨 TROUBLESHOOTING

### **Lỗi: "git is not recognized"**
→ Cài Git từ: https://git-scm.com/download/win

### **Lỗi: "fatal: not a git repository"**
→ Chạy lại: `git init`

### **Lỗi: "remote origin already exists"**
```powershell
git remote remove origin
git remote add origin https://github.com/YOUR_USERNAME/TrainingB.git
```

### **Lỗi: "failed to push some refs"**
```powershell
git pull origin main --allow-unrelated-histories
git push -u origin main
```

### **Lỗi: "Authentication failed"**
→ Dùng **Personal Access Token**, KHÔNG phải password!

---

## 📞 NẾU GẶP VẤN ĐỀ

Copy error message và báo cho tôi, tôi sẽ giúp fix! 😊
