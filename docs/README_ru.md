# 📨 Программная система, предназначенная для сотрудников почтовых отделений 

### Система представляет собой инструмент для управления подписными изданиями почтового отделения. Она собирает информацию о пользователях, изданиях, персонале и обслуживаемых участках. Система позволяет оформлять подписки, отслеживать доставку, а также формировать отчеты о количестве подписчиков, изданий и почтальонов.

## 📡 Технологии
+ **WPF**
+ **ADO.NET** (хранимые процедуры, функции)
+ **Entity Framework**
+ **Паттерн MVVM**
+ **Git**, **Github**, методология **Scrum** управления проектом
+ **MailOfficeLibs** (Собственные библиотеки)

## ⚙️ Системные требования 
### 🖳 Минимальные:
* **Операционная система:** 
    * Windows 10 (версия 22H2+) 
    * Windows 11 (версия 24H2, 23H2, 22H2 Ent/Edu)
* **Оперативная память**: 300 мб
* **Дисковое пространство**: 140 мб 
* **Разрешение экрана**: 1280x1024 или выше (рекомендуется 1920x1080 или выше)
* **Поддерживаемая архитектура**: Any CPU

## 📌 Цели и задачи системы 
### Разработать программную систему, которая автоматизирует и оптимизирует ключевые процессы управления подписными изданиями и доставкой в почтовом отделении, обеспечивая эффективное и точное обслуживание подписчиков, почтальонов и руководителей.

## 📈 Требования к программе
### **Создать программную систему, которая:**

* 📊 **Обеспечивает учет и управление данными:**
   * Позволяет хранить, редактировать и извлекать информацию о подписчиках, почтовых изданиях (газеты, журналы), почтальонах и обслуживаемых ими участках.
   * Обеспечивает целостность и достоверность данных о подписках (дата начала, срок), адресах и обслуживании.
* 🤖 **Автоматизирует процессы подписки:**
   * Предоставляет оператору почтовой связи инструменты для оформления новых подписок и добавления новых изданий в систему.
   * Генерирует квитанции для клиентов с указанием общей стоимости, списка выписанных изданий и срока подписки.
* 🛠️ **Поддерживает управление персоналом:**
   * Позволяет заведующему почтового отделения управлять персоналом почтальонов (прием на работу и увольнение), обеспечивая при этом непрерывное обслуживание всех участков.
   * Обеспечивает связность почтальонов с обслуживаемыми участками и подписчиками.
* 🛢️ **Предоставляет возможность формирования различных запросов к базе данных:**
   * Позволяет извлекать необходимую информацию для оперативного управления и контроля:
     * Определение наименования и количества всех экземпляров изданий.
     * Поиск почтальона по адресу подписчика.
     * Получение списка изданий, выписанных конкретным подписчиком.
     * Определение количества работающих почтальонов.
     * Выявление участка с максимальным количеством экземпляров подписных изданий.
   * Расчет среднего срока подписки по каждому изданию.
   * Получение справки о количестве подписчиков, газет и журналов на текущий момент.
* 📋 **Формирует отчеты для анализа и контроля:**
   * Генерирует детализированный отчет о доставке почты, упорядоченный по участкам.
   * **Отчет содержит:**
     * Информацию о почтальонах (ФИО).
     * Перечень доставляемых изданий (индекс, название, адрес, срок).
     * Средний срок подписки по изданию.
     * Количество экземпляров каждого издания.
     * Количество различных подписных изданий на участке.
     * Включает общую информацию о работе отделения:
       * Количество работающих почтальонов.
       * Количество обслуживаемых участков.
       * Количество доставляемых изданий.

##
![Static Badge](https://img.shields.io/badge/release-v1.0%20beta-yellow)
![GitHub commit activity (branch)](https://img.shields.io/github/commit-activity/m/Juimun/mail-office-project/master)
![GitHub last commit](https://img.shields.io/github/last-commit/Juimun/mail-office-project)







