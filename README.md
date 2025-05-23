# YJ_Jump_Project


---

## 개요

Unity 3D로 구현한 점핑액션 게임 프로젝트입니다.

---

## 주요 기능

### 플레이어
- 이동 : `W``A``S``D`
- 점프 : `스페이스 바(Space)`
- 달리기(추가 예정) : `쉬프트 키(Shift)`
  - 스태미나를 소모합니다.
- 상호작용 : `E`
- 퀵슬롯 선택 : `1~8`
- 아이템 사용 : `F`
- 시점 변환 : `Q`

### 아이템
- 스테이크 : 체력 회복
- 고추 : 속도 증가
- 당근 : 점프력 증가

### 플랫폼
- 점프패드 :
- 
  ![image](https://github.com/user-attachments/assets/dacf0a0f-17f6-4874-9003-5a4fa2952eca)
  - 플레이어가 닿는 즉시 위쪽 방향으로 플레이어를 발사합니다.
- 레이저 플랫폼 :
- 
  ![image](https://github.com/user-attachments/assets/268b059e-a182-4138-936b-a75facd8879e)
  - 플레이어가 레이저에 닿으면 위에서 함정이 떨어지는 플랫폼입니다.
- 움직이는 플랫폼 :
- 
  ![image](https://github.com/user-attachments/assets/dab39f2b-d917-419f-b6cd-6dc84d94096e)
  - 지정된 위치로 움직임을 반복하는 플랫폼입니다.
- 슈팅 플랫폼 :
- 
  ![image](https://github.com/user-attachments/assets/37fc3a89-9ca9-4713-9610-633c24b06bf6)
  - 일정 시간이 지난 후 플레이어가 바라보는 방향으로 플레이어를 발사합니다.

### UI
![image](https://github.com/user-attachments/assets/c173daa0-1d28-481e-acb0-12fa529de3f2)
- 좌측 하단 : 아이템 사용 시 버프 지속시간을 표시합니다.
- 중앙 하단 : 퀵슬롯입니다. 현재 소지중인 아이템과 아이템의 수량을 표시합니다.
- 우측 하단 : 체력과 스태미나를 표시합니다.

### 오브젝트
- 곰 (추가 예정)
  - 특정 위치에 도달 시 플레이어를 따라오는 적대적 오브젝트입니다.
---

## 향후 계획
- 달리기 기능 추가
- 적대적 오브젝트 추가
- 맵 추가 구현
